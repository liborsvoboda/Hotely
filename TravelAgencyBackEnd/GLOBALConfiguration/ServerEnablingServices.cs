namespace UbytkacBackend {

    public class ServerEnablingServices {

        internal static void EnableCors(ref IApplicationBuilder app) {
            /*
            app.UseCors(
                    builder =>
                    {
                        var allowedDomains = new[] { "http://localhost:5000", "http://nomad.ubytkac.cz:5000", "http://localhost:8080" };

                        builder
                        .WithOrigins(allowedDomains)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    }
                );
            */

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()

            //.AllowCredentials()
            );
        }

        internal static void EnableEndpoints(ref IApplicationBuilder app) {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=ServerCorePages}/{action=Index}/{id?}");
            });
        }
    }
}