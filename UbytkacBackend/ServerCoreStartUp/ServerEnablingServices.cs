using EasyData.Services;
using Microsoft.CodeAnalysis;
using Snickler.RSSCore.Caching;
using Snickler.RSSCore.Extensions;
using Snickler.RSSCore.Models;

namespace UbytkacBackend.ServerCoreConfiguration {

    /// <summary>
    /// Server Core Enabling Settings of Security, API Communications, Technologies, Modules,Rules,
    /// Rights, Rules, Rights, Conditions, Cors, Cookies, Formats, Services, Logging, etc..
    /// </summary>
    public class ServerEnablingServices {

        /// <summary>
        /// Enable Server Logging in Debug Mode
        /// </summary>
        internal static void EnableLogging(ref IApplicationBuilder app) {
            if (ServerRuntimeData.DebugMode) { app.UseHttpLogging(); }
        }

        /// <summary>
        /// Server Cors Configuration
        /// </summary>
        internal static void EnableCors(ref IApplicationBuilder app) {
            if (ServerConfigSettings.ServerCorsEnabled) {
                app.UseCors(x => {
                    List<ServerCorsDefAllowedOriginList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        data = new hotelsContext().ServerCorsDefAllowedOriginLists.Where(a => a.Active).ToList();
                    }

                    if (ServerConfigSettings.ServerCorsAllowAnyHeader) { x.AllowAnyHeader(); }
                    if (ServerConfigSettings.ServerCorsAllowAnyMethod) { x.AllowAnyMethod(); }
                    if (ServerConfigSettings.ServerCorsAllowCredentials) { x.AllowCredentials(); }
                    if (ServerConfigSettings.ServerCorsAllowAnyOrigin) { x.AllowAnyOrigin(); }
                    else if (data.Any()) { string[] allowedDomains = data.Select(a => a.AllowedDomain).ToArray(); x.WithOrigins(allowedDomains); }
                });
            };
        }

        /// <summary>
        /// Server WebSocket Configuration
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        internal static void EnableWebSocket(ref IApplicationBuilder app) {
            if (ServerConfigSettings.WebSocketEngineEnabled) {
                var webSocketOptions = new WebSocketOptions() {
                    KeepAliveInterval = TimeSpan.FromHours(ServerConfigSettings.WebSocketTimeoutMin),
                };
                app.UseWebSockets(webSocketOptions);
            }
        }



        internal static void EnableRssFeed(ref IApplicationBuilder app) {
            if (ServerConfigSettings.WebRSSFeedsEnabled) {
                app.UseRSSFeed("/feed", new RSSFeedOptions {
                    Title = ServerConfigSettings.ConfigCoreServerRegisteredName + " RSS Feed",
                    Copyright = "2023 " + ServerConfigSettings.ConfigCoreServerRegisteredName,
                    Description = "RSS Feed with Company Portfolio",
                    ManagingEditor = ServerConfigSettings.ConfigManagerEmailAddress,
                    Webmaster = ServerConfigSettings.EmailerServiceEmailAddress,
                    Url = new Uri(ServerConfigSettings.ServerPublicUrl),
                    Caching = new MemoryCacheProvider { CacheDuration = TimeSpan.FromDays(5), Key = "RSSCacheKey" }
                });
            }
        }

        /// <summary>
        /// Server Endpoints Configuration
        /// </summary>
        internal static void EnableEndpoints(ref IApplicationBuilder app) {
            app.UseEndpoints(endpoints => {
                if (ServerConfigSettings.ModuleWebDataManagerEnabled) { endpoints.MapEasyData(options => { options.UseDbContext<hotelsContext>(); }); }

                endpoints.MapControllers();

                if (ServerConfigSettings.WebRazorPagesEngineEnabled) {
                    endpoints.MapRazorPages();
                    endpoints.MapControllerRoute(name: "default", pattern: "{controller=ServerCorePages}/{action=Index}/{id?}");
                }

                if (ServerConfigSettings.ModuleHealthServiceEnabled) {
                    endpoints.MapHealthChecks("/HealthResultService",
                        new HealthCheckOptions() {
                            Predicate = p => true,
                            ResultStatusCodes = { [HealthStatus.Healthy] = StatusCodes.Status200OK, [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError, [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable },
                            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                            AllowCachingResponses = false
                        }
                        );
                }

            });


            if (ServerConfigSettings.ModuleHealthServiceEnabled) {
                app.UseHealthChecks("/HealthResultService");
                app.UseHealthChecksUI(setup => {
                    setup.UIPath = ServerConfigSettings.ModuleHealthServicePath.StartsWith("/") ? ServerConfigSettings.ModuleHealthServicePath : "/" + ServerConfigSettings.ModuleHealthServicePath;
                    setup.AsideMenuOpened = true;
                    setup.PageTitle = ServerConfigSettings.ConfigCoreServerRegisteredName;
                    setup.AddCustomStylesheet("ServerCoreDefinitions/Styles/HealthChecksUI.css");
                });
            }
        }
    }
}