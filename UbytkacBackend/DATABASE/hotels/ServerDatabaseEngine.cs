using System.Data;

namespace UbytkacBackend.ServerConfiguration {

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
    }
    
    
    /// <summary>
    /// Database Context Extensions for All Types Procedures For Retun Data in procedure can be
    /// Simple SELECT * XXX and you Create Same Class for returned DataSet
    /// </summary>
    public static class DatabaseContextExtensions {

        public static List<T> UbytkacBackendCollectionFromSql<T>(this hotelsContext dbContext, string sql) where T : class, new() {
            using var cmd = dbContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            try {
                List<T> results = null;
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                table.Load(cmd.ExecuteReader());
                results = DbOperations.BindList<T>(table).ToList();

                return results;
            } catch (Exception Ex) { CoreOperations.SendEmail(new MailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); } finally { cmd.Connection.Close(); }
            return new List<T>();
        }
    }
}