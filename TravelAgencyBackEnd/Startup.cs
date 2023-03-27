using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Microsoft.AspNetCore.HttpOverrides;

namespace TravelAgencyBackEnd
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {

            #region Server Core & Security
            ServerCoreConfiguration.ConfigureCookie(ref services);
            //ServerCoreConfiguration.ConfigureAllowAllCors(ref services);
            ServerCoreConfiguration.ConfigureControllers(ref services);
            ServerCoreConfiguration.ConfigureAuthentication(ref services);
            services.AddEndpointsApiExplorer();
            StripeConfiguration.ApiKey = "sk_test_51Ix5jxB3kuAuBTpumfFOUXqYLSAiDcCYrIejgIX48knYENVJdkaB42V1VdqBPIuvM9KbuWwSrkfMNxkjYSrQucx500FLoljwSC";
            #endregion

            #region Server Data Segment
            ServerCoreConfiguration.ConfigureScopes(ref services);
            ServerCoreConfiguration.ConfigureDatabaseContext(ref services);
            ServerCoreConfiguration.ConfigureThirdPartyApi(ref services);
            ServerCoreConfiguration.ConfigureLogging(ref services);
            #endregion


            #region Server Modules
            ServerModulesConfiguration.ConfigureSwagger(ref services);
            #endregion

        }



        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
