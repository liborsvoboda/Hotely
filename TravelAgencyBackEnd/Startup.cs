using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using UbytkacBackend.ServerCoreConfiguration;
using UbytkacBackend.ServerCoreStructure;

namespace UbytkacBackend {

    /// <summary>
    /// Server Startup Definitions
    /// </summary>
    public class Startup {

        /// <summary>
        /// Server Core: Main Configure of Server Services, Technologies, Modules, etc..
        /// </summary>
        /// <param name="services"></param>
        /// <returns>void.</returns>
        public void ConfigureServices(IServiceCollection services) {

            #region Server Data Segment
            //DB first for Configuration
            ServerConfigurationServices.ConfigureDatabaseContext(ref services);

            ServerConfigurationServices.ConfigureScoped(ref services);
            ServerConfigurationServices.ConfigureThirdPartyApi(ref services);
            ServerConfigurationServices.ConfigureLogging(ref services);

            #endregion Server Data Segment

            #region Server WebServer

            ServerConfigurationServices.ConfigureServerWebPages(ref services);
            ServerConfigurationServices.ConfigureFTPServer(ref services);
            if (ServerConfigSettings.WebBrowserContentEnabled) { services.AddDirectoryBrowser(); }

            services.AddSession(options => {
                options.Cookie.Name = "SessionCookie";
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            #endregion Server WebServer

            #region Server Core & Security Web

            ServerConfigurationServices.ConfigureCookie(ref services);
            ServerConfigurationServices.ConfigureControllers(ref services);
            ServerConfigurationServices.ConfigureAuthentication(ref services);
            ServerConfigurationServices.ConfigureLetsEncrypt(ref services);
            services.AddHttpContextAccessor();
            services.AddResponseCompression();
            services.AddResponseCaching();
            services.AddMemoryCache();
            services.AddEndpointsApiExplorer();
            ServerConfigurationServices.ConfigureWebSocketLoggerMonitor(ref services);
            ServerConfigurationServices.ConfigureRSSfeed(ref services);

            #endregion Server Core & Security Web

            #region Server Modules

            ServerModules.ConfigureScheduler(ref services);
            ServerModules.ConfigureSwagger(ref services);
            ServerModules.ConfigureHealthCheck(ref services);
            ServerModules.ConfigureDocumentation(ref services);
            ServerModules.ConfigureLiveDataMonitor(ref services);
            ServerModules.ConfigureDBEntitySchema(ref services);
            #endregion Server Modules

            ServerConfigurationServices.ConfigureSingletons(ref services);
            ServerConfigurationServices.ConfigureCentralServicesProviders(ref services);
        }


        /// <summary>
        /// Server Core: Main Enabling of Server Services, Technologies, Modules, etc..
        /// </summary>
        /// <param name="app">           </param>
        /// <param name="serverLifetime"></param>
        public void Configure(IApplicationBuilder app, IHostApplicationLifetime serverLifetime) {
            serverLifetime.ApplicationStarted.Register(ServerOnStarted);
            serverLifetime.ApplicationStopping.Register(ServerOnStopping);
            serverLifetime.ApplicationStopped.Register(ServerOnStopped);

            ServerEnablingServices.EnableLogging(ref app);
            ServerModulesEnabling.EnableSwagger(ref app);
            ServerModulesEnabling.EnableLiveDataMonitor(ref app);
            ServerModulesEnabling.EnableDBEntitySchema(ref app);

            if (ServerConfigSettings.ConfigServerStartupOnHttps) {
                app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });
                app.UseCertificateForwarding();
            }

            //Root Page To Server Index
            //app.Use(async (context, next) => { if (context.Request.Path.Value == "/") { context.Request.Path = BackendServer.ServerRuntimeData.SpecialUserWebRootPath; } await next(); });

            //Check API Modules Right For Token or User Authentificated
            app.Use(async (context, next) => {
                string RequestPath = context.Request.Path.ToString().ToLower();

                //include user from cookie
                ServerWebPagesToken? serverWebPagesToken = null; string token = context.Request.Cookies.FirstOrDefault(a => a.Key == "ApiToken").Value;
                if (context.Request.Headers.Authorization.ToString().Length > 0) {
                    token = context.Request.Headers.Authorization.ToString().Substring(7);
                }

                if (token != null) {
                    serverWebPagesToken = CoreOperations.CheckTokenValidityFromString(token);
                    if (serverWebPagesToken.IsValid) { context.User.AddIdentities(serverWebPagesToken.UserClaims.Identities); context.Items.Add(new KeyValuePair<object, object>("ServerWebPagesToken", serverWebPagesToken)); }
                }

                //Verify Access To Module and Go Next Or Redirect to RedirectUrl
                ServerModuleAndServiceList? RequestedModule = new hotelsContext().ServerModuleAndServiceLists.FirstOrDefault(a => a.UrlSubPath.ToLower().StartsWith(RequestPath));
                if (RequestedModule == null) { await next(); }
                else if (RequestedModule != null && (!RequestedModule.RestrictedAccess
                || (RequestedModule.RestrictedAccess && serverWebPagesToken != null && serverWebPagesToken.IsValid && RequestedModule.AllowedRoles.Split(",").ToList().Contains(serverWebPagesToken.UserClaims.FindFirstValue(ClaimTypes.Role))))) { context.Response.StatusCode = 200; await next(); }
                else { context.Response.Cookies.Append("RequestedModulePath", RequestPath); context.Response.Cookies.Append("RequestedModuleAccess", RequestedModule.AllowedRoles); context.Request.Path = RequestedModule.RedirectPathOnError; await next(); }
            });



            //404 Redirect Central One Page Portal not For Files
            //app.Use(async (context, next) => { await next(); if (context.Response.StatusCode == 404 && ServerConfigSettings.RedirectOnPageNotFound && !context.Request.Path.ToString().Split("/").Last().Contains(".")) {
            //        // Redirect To Portal Or Module Or Login Module
            //        context.Request.Path = ServerConfigSettings.RedirectPath;
            //        context.Response.StatusCode = 200; await next();
            //    }
            //});



            //404 Redirect Central One Page Portal not For Files
            app.Use(async (context, next) => {
                await next(); if (context.Response.StatusCode == 404 && ServerConfigSettings.RedirectOnPageNotFound && !context.Request.Path.ToString().Split("/").Last().Contains(".")) {
                    string requestPath = context.Request.Path;
                    string? RequestedModulePath = context.Request.Cookies.FirstOrDefault(a => a.Key.ToString() == "RequestedModulePath").Value?.ToString(); string? RequestModuleAccess = context.Request.Cookies.FirstOrDefault(a => a.Key.ToString() == "RequestedModuleAccess").Value?.ToString();

                    //include user from cookie
                    ServerWebPagesToken? serverWebPagesToken = null; string token = context.Request.Cookies.FirstOrDefault(a => a.Key == "ApiToken").Value;
                    if (token != null) { serverWebPagesToken = CoreOperations.CheckTokenValidityFromString(token); if (serverWebPagesToken.IsValid) { context.User.AddIdentities(serverWebPagesToken.UserClaims.Identities); try { context.Items.Add(new KeyValuePair<object, object>("ServerWebPagesToken", serverWebPagesToken)); } catch { } } }

                    //Check Redirected Login Return to API
                    if (RequestedModulePath != null && serverWebPagesToken != null && serverWebPagesToken.IsValid && RequestModuleAccess.Split(",").Contains(serverWebPagesToken.UserClaims.FindFirstValue(ClaimTypes.Role))) { context.Request.Path = RequestedModulePath; context.Response.StatusCode = 200; await next(); }

                    //include module && Login Module
                    ServerModuleAndServiceList? RequstedModule = RequestedModulePath == null ? new hotelsContext().ServerModuleAndServiceLists.FirstOrDefault(a => a.UrlSubPath.ToLower().StartsWith(requestPath)) :
                    new hotelsContext().ServerModuleAndServiceLists.FirstOrDefault(a => a.UrlSubPath.ToLower().StartsWith(RequestedModulePath.ToLower()));

                    //Redirect To Show Module or Login Page
                    if (RequstedModule != null) {
                        ServerModuleAndServiceList? loginmodule = new hotelsContext().ServerModuleAndServiceLists.FirstOrDefault(a => a.IsLoginModule);
                        context.Items.Add(new KeyValuePair<object, object>("ServerModule", RequstedModule)); context.Items.Add(new KeyValuePair<object, object>("LoginModule", loginmodule));
                    }

                    // Redirect To Portal Or Module Or Login Module
                    context.Request.Path = ServerConfigSettings.RedirectPath;
                    context.Response.StatusCode = 200; await next();
                }
            });







            app.UseExceptionHandler("/Error");
            app.UseRouting();
            app.UseDefaultFiles();

            app.UseHsts();
            if (ServerConfigSettings.ConfigServerStartupOnHttps) { app.UseHttpsRedirection(); }
            app.UseStaticFiles(new StaticFileOptions { ServeUnknownFileTypes = true, HttpsCompression = HttpsCompressionMode.Compress });
            app.UseCookiePolicy();
            app.UseSession();
            app.UseResponseCaching();
            app.UseResponseCompression();
            app.UseAuthentication();
            app.UseAuthorization();

            ServerEnablingServices.EnableCors(ref app);
            ServerEnablingServices.EnableWebSocket(ref app);
            ServerEnablingServices.EnableEndpoints(ref app);
            ServerModulesEnabling.EnableDocumentation(ref app);
            ServerEnablingServices.EnableRssFeed(ref app);

            if (ServerConfigSettings.WebBrowserContentEnabled) {
                //These commented setting enable full browsing
                //app.UseDirectoryBrowser(); app.UseFileServer(enableDirectoryBrowsing: true);

                List<ServerBrowsablePathList> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().ServerBrowsablePathLists.Where(a => a.Active).ToList();
                }

                data.ForEach(path => {
                    try {
                        app.UseFileServer(new FileServerOptions {
                            FileProvider = new PhysicalFileProvider(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", path.WebRootPath)),
                            RequestPath = "/" + (path.AliasPath != null && path.AliasPath.Length > 0 ? path.AliasPath : path.WebRootPath),
                            EnableDirectoryBrowsing = true,
                        });
                    } catch (Exception Ex) { CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); }
                });
            }

            if (ServerConfigSettings.WebMvcPagesEngineEnabled) { app.UseMvcWithDefaultRoute(); }
            if (ServerConfigSettings.RedirectOnPageNotFound) {
                try { app.UsePathBase(ServerConfigSettings.RedirectPath); } catch (Exception Ex) { CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); }
            }
            if (ServerConfigSettings.ModuleWebDataManagerEnabled) { app.UseEasyData(); }
        }

        /// <summary>
        /// Server Core Enabling Disabling Hosted Services
        /// </summary>
        private void ServerOnStarted() => ServerRuntimeData.ServerCoreStatus = ServerStatuses.Running.ToString();

        private void ServerOnStopping() => ServerRuntimeData.ServerCoreStatus = ServerStatuses.Stopping.ToString();

        private void ServerOnStopped() => ServerRuntimeData.ServerCoreStatus = ServerStatuses.Stopped.ToString();
    }
}