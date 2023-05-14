//this is automatic solution

namespace TravelAgencyBackEnd.ServerConfiguration {

    /// <summary>
    /// Server Main Database Settings Here is Included ScaffoldContext which is connected via Visual
    /// Studio Tool Here can Be added customization which are not on the server Here is Extended the
    /// Context with Insert News Functionality
    /// </summary>
    public partial class hotelsContext : ScaffoldContext {

        public hotelsContext() {
        }

        public hotelsContext(DbContextOptions<ScaffoldContext> options)
            : base(options) {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.ConfigureLoggingCacheTime(TimeSpan.FromMinutes(30));
                optionsBuilder.EnableServiceProviderCaching(false);

                optionsBuilder.UseSqlServer("Server=95.183.52.33;Database=hotels;User ID=sa;Password=Hotel2023+;TrustServerCertificate=True;",
                      x => x.MigrationsHistoryTable("MigrationsHistory", "dbo"));
            }
        }

        /// <summary>
        /// Return User From API Request if Exist other null
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public int? GetApiUser(HttpContext? httpContext) {
            int? requestUserId = null;
            try
            {
                requestUserId = (httpContext != null && httpContext.User != null)
                    ? int.Parse(httpContext.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.PrimarySid).Value)
                    : null;
            }
            catch { }
            return requestUserId;
        }
    }
}