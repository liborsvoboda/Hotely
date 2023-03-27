using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.WebSockets;

namespace TravelAgencyBackEnd
{

    public class ServerEnablingServices
    {


        internal static void EnableCors(ref IApplicationBuilder app)
        {
            /*
            return app.UseCors(
                    builder =>
                    {
                        var allowedDomains = new[] { "http://localhost", "http://localhost:4200", "http://localhost:8080" };

                        builder
                        .WithOrigins(allowedDomains)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    }
                );
            });
            */

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            //.AllowCredentials()
            );
        }


        internal static void EnableEndpoints(ref IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
