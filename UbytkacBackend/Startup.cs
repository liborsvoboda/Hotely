using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;


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

            //Root Server Page To Default Path, Other Is Taken From Static Files
            app.Use(async (context, next) => { if (context.Request.Path.Value == "/" && ServerConfigSettings.RedirectOnPageNotFound) { context.Request.Path = BackendServer.ServerRuntimeData.SpecialUserWebRootPath; } await next(); });

            //Check Fouded Server API Modules Right For Token or User Authentificated
            app.Use(async (context, next) => {
                context.Response.StatusCode = StatusCodes.Status200OK; ServerModuleAndServiceList? serverModule = null;

                /* Login Request Detected ON PHYSICAL MODULE - Ignored */
                string requestedModulePath = null;
                try { requestedModulePath = context.Request.Cookies.FirstOrDefault(a => a.Key.ToString() == "IsLoginRequest").Value?.ToString(); } catch { }
                if (requestedModulePath == "correct") { try { context.Response.Cookies.Delete("IsLoginRequest"); context.Response.Cookies.Delete("RequestedModulePath"); } catch { } await next(); }
                else {

                    //Check API module Request is Redirected or Its First contact - Read Module and Token
                    ServerWebPagesToken? serverWebPagesToken = null; string token = context.Request.Cookies.FirstOrDefault(a => a.Key == "ApiToken").Value;
                    if (token == null && context.Request.Headers.Authorization.ToString().Length > 0) { token = context.Request.Headers.Authorization.ToString().Substring(7); }
                    serverModule = new hotelsContext().ServerModuleAndServiceLists.FirstOrDefault(a => a.UrlSubPath.ToLower().Replace("/", "").StartsWith(context.Request.Path.Value.ToLower().Replace("/", "")));


                    if (serverModule == null) { /* server API module wuthout Control go Next */ await next(); }
                    else if (serverModule != null && !serverModule.RestrictedAccess) { /* server API module is Free go Next */ try { context.Items.Add(new KeyValuePair<object, object>("ServerModule", serverModule)); } catch { } await next(); }
                    else if (serverModule != null && serverModule.RestrictedAccess && token != null) {
                        serverWebPagesToken = CoreOperations.CheckTokenValidityFromString(token);
                        if (serverWebPagesToken.IsValid && serverModule.AllowedRoles != null && serverModule.AllowedRoles.Split(",").ToList().Contains(serverWebPagesToken.UserClaims.FindFirstValue(ClaimTypes.Role))) { /* Token is OK go next*/
                            try { context.Items.Add(new KeyValuePair<object, object>("ServerModule", serverModule)); } catch { }
                            await next();
                        }
                        else { /* Token is not correct denied = redirect to login again */
                            ServerModuleAndServiceList? loginmodule = new hotelsContext().ServerModuleAndServiceLists.FirstOrDefault(a => a.IsLoginModule);
                            try { context.Items.Add(new KeyValuePair<object, object>("ServerModule", serverModule)); } catch { }
                            try { context.Items.Add(new KeyValuePair<object, object>("LoginModule", loginmodule)); } catch { }
                            try { context.Response.Cookies.Append("IsLoginRequest", "correct"); } catch { }
                            try { context.Response.Cookies.Append("RequestedModulePath", context.Request.Path.Value); } catch { }
                            context.Request.Path = serverModule.RedirectPathOnError; await next();
                        }
                    }
                    else { /* unspecified problem = redirect to login */
                        ServerModuleAndServiceList? loginmodule = new hotelsContext().ServerModuleAndServiceLists.FirstOrDefault(a => a.IsLoginModule);
                        try { context.Items.Add(new KeyValuePair<object, object>("ServerModule", serverModule)); } catch { }
                        try { context.Items.Add(new KeyValuePair<object, object>("LoginModule", loginmodule)); } catch { }
                        try { context.Response.Cookies.Append("IsLoginRequest", "correct"); } catch { }
                        try { context.Response.Cookies.Append("RequestedModulePath", context.Request.Path.Value); } catch { }
                        context.Request.Path = serverModule.RedirectPathOnError; await next();
                    }
                }
            });



            //404 Redirect Central One Page Portal not For Files, Files has 404 Filename format is ignored
            app.Use(async (context, next) => {
                await next();
                if (context.Response.StatusCode == StatusCodes.Status404NotFound && !context.Request.Path.ToString().Split("/").Last().Contains(".")) {
                    string requestedModulePath = null; string isLoginRequest = null; ServerWebPagesToken? serverWebPagesToken = null; ServerModuleAndServiceList? requstedModule = null;

                    //START SECTION FOR SHOW UNIQUE DYNAMIC GLOBAL FORMS

                    //Resolve Virtual Login Form Request 
                    try { isLoginRequest = context.Request.Cookies.FirstOrDefault(a => a.Key.ToString() == "IsLoginRequest").Value?.ToString(); } catch { }
                    if (isLoginRequest == "correct") {
                        context.Request.Path = BackendServer.ServerRuntimeData.SpecialUserWebRootPath; context.Response.StatusCode = StatusCodes.Status200OK;

                    }
                    else { //START SECTION FOR STANDALONE DYNAMIC MODULES 
                        //Prepare path for check if is module
                        try { requestedModulePath = context.Request.Cookies.FirstOrDefault(a => a.Key.ToString() == "RequestedModulePath").Value?.ToString(); } catch { }
                        if (requestedModulePath == null) { requestedModulePath = context.Request.Path.Value; }
                        requstedModule = new hotelsContext().ServerModuleAndServiceLists.FirstOrDefault(a => a.UrlSubPath.ToLower().Replace("/", "").StartsWith(requestedModulePath.ToLower().Replace("/", "")));

                        //Not monitored path or not limited
                        if (requstedModule == null || (requstedModule != null && !requstedModule.RestrictedAccess)) {
                            try { context.Items.Add(new KeyValuePair<object, object>("ServerModule", requstedModule)); } catch { }
                            context.Request.Path = BackendServer.ServerRuntimeData.SpecialUserWebRootPath; context.Response.StatusCode = StatusCodes.Status200OK;

                            //END SECTION FOR STANDALONE DYNAMIC MODULES 
                        }
                        else {//START LOGIN PROCESS

                            //checking existing login and include to request
                            string token = context.Request.Cookies.FirstOrDefault(a => a.Key == "ApiToken").Value;
                            if (token == null && context.Request.Headers.Authorization.ToString().Length > 0) { token = context.Request.Headers.Authorization.ToString().Substring(7); }
                            if (token != null) {
                                serverWebPagesToken = CoreOperations.CheckTokenValidityFromString(token);
                                if (serverWebPagesToken.IsValid) { context.User.AddIdentities(serverWebPagesToken.UserClaims.Identities); try { context.Items.Add(new KeyValuePair<object, object>("ServerWebPagesToken", serverWebPagesToken)); } catch { } }
                            }

                            //START MODULE CHECKING
                            if (requstedModule != null && requstedModule.RestrictedAccess && serverWebPagesToken != null && serverWebPagesToken.IsValid && requstedModule.AllowedRoles != null && requstedModule.AllowedRoles.Split(",").Contains(serverWebPagesToken.UserClaims.FindFirstValue(ClaimTypes.Role))) {
                                try { context.Items.Add(new KeyValuePair<object, object>("ServerModule", requstedModule)); } catch { }
                                context.Request.Path = BackendServer.ServerRuntimeData.SpecialUserWebRootPath; context.Response.StatusCode = StatusCodes.Status200OK;
                            }
                            else if (requstedModule != null) { //Redirect To Show Module or Login Page
                                ServerModuleAndServiceList? loginmodule = new hotelsContext().ServerModuleAndServiceLists.FirstOrDefault(a => a.IsLoginModule);
                                try { context.Items.Add(new KeyValuePair<object, object>("LoginModule", loginmodule)); } catch { }
                                try { context.Items.Add(new KeyValuePair<object, object>("ServerModule", requstedModule)); } catch { }
                                try { context.Response.Cookies.Append("IsLoginRequest", "correct"); } catch { }
                                try { context.Response.Cookies.Append("RequestedModulePath", requestedModulePath); } catch { }
                                context.Request.Path = BackendServer.ServerRuntimeData.SpecialUserWebRootPath; context.Response.StatusCode = StatusCodes.Status200OK;

                            } //Must be redirected to existing page everytime is Loaded by Module
                            else if (context.Request.Path != BackendServer.ServerRuntimeData.SpecialUserWebRootPath && ServerConfigSettings.RedirectOnPageNotFound) {
                                context.Request.Path = BackendServer.ServerRuntimeData.SpecialUserWebRootPath; context.Response.StatusCode = StatusCodes.Status200OK;
                            }
                        }//end of login
                    }//end of standalone section

                    await next();
                }
                else if (context.Response.StatusCode == StatusCodes.Status404NotFound && context.Request.Path != BackendServer.ServerRuntimeData.SpecialUserWebRootPath) {
                    context.Request.Path = BackendServer.ServerRuntimeData.SpecialUserWebRootPath; context.Response.StatusCode = StatusCodes.Status200OK; await next();
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
                            FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(ServerRuntimeData.Startup_path, ServerConfigSettings.DefaultStaticWebFilesFolder, path.WebRootPath)),
                            RequestPath = "/" + (path.AliasPath != null && path.AliasPath.Length > 0 ? path.AliasPath : path.WebRootPath),
                            EnableDirectoryBrowsing = true,
                        });
                    } catch (Exception Ex) { CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); }
                });
            }
           
            if (ServerConfigSettings.WebMvcPagesEngineEnabled) { app.UseMvcWithDefaultRoute(); }
            try { app.UsePathBase(BackendServer.ServerRuntimeData.SpecialUserWebRootPath); } catch (Exception Ex) { CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); }
            if (ServerConfigSettings.ModuleWebDataManagerEnabled) {app.UseEasyData();}

        }

        /// <summary>
        /// Server Core Enabling Disabling Hosted Services
        /// </summary>
        private void ServerOnStarted() => ServerRuntimeData.ServerCoreStatus = ServerStatuses.Running.ToString();

        private void ServerOnStopping() => ServerRuntimeData.ServerCoreStatus = ServerStatuses.Stopping.ToString();

        private void ServerOnStopped() => ServerRuntimeData.ServerCoreStatus = ServerStatuses.Stopped.ToString();
    }
}