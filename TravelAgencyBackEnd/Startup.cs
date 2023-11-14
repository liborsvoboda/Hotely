using Microsoft.Extensions.FileProviders;
using Westwind.AspNetCore.LiveReload;

namespace UbytkacBackend {

    public class Startup {

        public void ConfigureServices(IServiceCollection services) {

            #region Server Core & Security

            ServerCoreConfiguration.ConfigureCookie(ref services);
            //ServerCoreConfiguration.ConfigureAllowAllCors(ref services);

            services.AddLiveReload(config => {
                config.LiveReloadEnabled = true;
                config.FolderToMonitor = Path.Combine(ServerConfigSettings.StartupPath, "wwwroot");
                config.ClientFileExtensions = ".cshtml,.html,.md,.css,.js,.png,.json,.*,.svg,.htm,.html,.ts,.razor,.custom";
            });

            services.AddRazorPages().AddRazorPagesOptions(opt => { opt.RootDirectory = "/ServerCorePages"; });
            ServerCoreConfiguration.ConfigureControllers(ref services);
            ServerCoreConfiguration.ConfigureAuthentication(ref services);
            services.AddHttpContextAccessor();
            services.AddEndpointsApiExplorer();
            #endregion Server Core & Security

            #region Server Data Segment

            ServerCoreConfiguration.ConfigureScopes(ref services);
            ServerCoreConfiguration.ConfigureDatabaseContext(ref services);
            ServerCoreConfiguration.ConfigureThirdPartyApi(ref services);
            ServerCoreConfiguration.ConfigureLogging(ref services);

            #endregion Server Data Segment

            #region Server Modules

            ServerModulesConfiguration.ConfigureSwagger(ref services);

            #endregion Server Modules
        }

        /// <summary>
        /// Server Configuration Definition
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (Program.DebugMode) { app.UseDeveloperExceptionPage(); }

            ServerModulesEnabling.EnableSwagger(ref app);

            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

            app.Use(async (context, next) => { if (context.Request.Path.Value == "/") { context.Request.Path = "/Index"; } await next(); });
            app.Use(async (context, next) => { await next(); if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value)) { context.Request.Path = "/Index"; await next(); } });
            app.UseExceptionHandler("/Error");
            app.UseRouting();
            app.UseDefaultFiles();

            app.UseLiveReload();
            app.UseStaticFiles(new StaticFileOptions { ServeUnknownFileTypes = true }); 
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            ServerEnablingServices.EnableCors(ref app);
            ServerEnablingServices.EnableEndpoints(ref app);

            app.UsePathBase("/Index");
        }
    }
}