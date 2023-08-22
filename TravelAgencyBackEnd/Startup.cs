namespace UbytkacBackend {

    public class Startup {

        public void ConfigureServices(IServiceCollection services) {

            #region Server Core & Security

            ServerCoreConfiguration.ConfigureCookie(ref services);
            //ServerCoreConfiguration.ConfigureAllowAllCors(ref services);
            ServerCoreConfiguration.ConfigureControllers(ref services);
            ServerCoreConfiguration.ConfigureAuthentication(ref services);
            services.AddEndpointsApiExplorer();
            //StripeConfiguration.ApiKey = "51Ix5jxB3kuAuBTpumfFOUXqYLSAiDcCYrIejgIX48knYENVJdkaB42V1VdqBPIuvM9KbuWwSrkfMNxkjYSrQucx500FLoljwSC";

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

            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
            app.UseExceptionHandler("/Error");
            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            ServerEnablingServices.EnableCors(ref app);
            ServerEnablingServices.EnableEndpoints(ref app);
        }
    }
}