using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;

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

                optionsBuilder.UseSqlServer(ServerConfigSettings.DatabaseConnectionString,
                      x => x.MigrationsHistoryTable("MigrationsHistory", "dbo"));
            }
        }
    }
    
    
    /// <summary>
    /// Database Context Extensions for All Types Procedures For Retun Data in procedure can be
    /// Simple SELECT * XXX and you Create Same Class for returned DataSet
    /// </summary>
    public static class DatabaseContextExtensions {

        public static DataView EasyITCenterCollectionFromSql(this hotelsContext hotelsContext, Type type, string sql) {
            using var cmd = hotelsContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            if (cmd.Connection?.State != ConnectionState.Open)
                cmd.Connection?.Open();
            try {
                DataView? results = null;
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                table.Load(cmd.ExecuteReader());
                results = (table.AsDataView());

                return results;
            } catch (Exception Ex) { CoreOperations.SendEmail(new SendMailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); } finally { cmd.Connection?.Close(); }
            return new DataView();
        }


        public static List<T> EasyITCenterCollectionFromSql<T>(this hotelsContext hotelsContext, string sql) where T : class, new() {
            using var cmd = hotelsContext.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            if (cmd.Connection?.State != ConnectionState.Open)
                cmd.Connection?.Open();
            try {
                List<T> results = null;
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                table.Load(cmd.ExecuteReader());
                results = DbOperations.BindList<T>(table).ToList();

                return results;
            } catch (Exception Ex) { CoreOperations.SendEmail(new SendMailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); } finally { cmd.Connection?.Close(); }
            return new List<T>();
        }

        public static IQueryable Set(this hotelsContext context, Type T) {
            MethodInfo method = typeof(hotelsContext).GetMethod(nameof(hotelsContext.Set), BindingFlags.Public | BindingFlags.Instance);
            method = method.MakeGenericMethod(T);
            return method.Invoke(context, null) as IQueryable;
        }

        public static IQueryable<T> Set<T>(this hotelsContext context) {
            MethodInfo method = typeof(hotelsContext).GetMethod(nameof(hotelsContext.Set), BindingFlags.Public | BindingFlags.Instance);
            method = method.MakeGenericMethod(typeof(T));
            return method.Invoke(context, null) as IQueryable<T>;
        }


        public static object? GetDbSet(hotelsContext db, Type T) {
            MethodInfo method = typeof(hotelsContext).GetMethod(nameof(hotelsContext.Set), BindingFlags.Public | BindingFlags.Instance);
            method = method.MakeGenericMethod(T);
            return method.Invoke(Set(db, T), null);
        }


        public static DbSet<T> GetDbSet<T>(hotelsContext db) where T : class {
            return db.Set<T>();
        }


        public static DbTransaction GetDbTransaction(this hotelsContext source) {
            return (source as IInfrastructure<DbTransaction>).Instance;
        }

        public static DbTransaction GetDbTransaction(this IDbContextTransaction source) {
            return (source as IInfrastructure<DbTransaction>).Instance;
        }

        public static object? ExecuteScalar(this hotelsContext context,
        string sql, List<DbParameter> parameters = null,
        CommandType commandType = CommandType.Text,
        int? commandTimeOutInSeconds = null) {
            Object? value;
            try {
                using (var cmd = context.Database.GetDbConnection().CreateCommand()) {

                    if (cmd.Connection?.State != ConnectionState.Open) {
                        cmd.Connection?.Open();
                    }
                    cmd.CommandText = sql;
                    cmd.CommandType = commandType;
                    if (commandTimeOutInSeconds != null) {
                        cmd.CommandTimeout = (int)commandTimeOutInSeconds;
                    }
                    if (parameters != null) {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    value = cmd.ExecuteScalar();
                    cmd.Connection?.Close();
                }
                return value;

            } catch (Exception Ex) { CoreOperations.SendEmail(new SendMailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); }
            return new object();

        }


        public static int ExecuteNonQuery(this hotelsContext context, string command, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null) {
            try {
                using (var cmd = context.Database.GetDbConnection().CreateCommand()) {
                    if (cmd.Connection?.State != ConnectionState.Open) {
                        cmd.Connection?.Open();
                    }
                    var currentTransaction = context.Database.CurrentTransaction;
                    if (currentTransaction != null) {
                        cmd.Transaction = currentTransaction.GetDbTransaction();
                    }
                    cmd.CommandText = command;
                    cmd.CommandType = commandType;
                    if (commandTimeOutInSeconds != null) {
                        cmd.CommandTimeout = (int)commandTimeOutInSeconds;
                    }
                    if (parameters != null) {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    int value = cmd.ExecuteNonQuery();
                    //cmd.Connection?.Close();
                    return value;
                }
            } catch (Exception Ex) { CoreOperations.SendEmail(new SendMailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); }
            return new int();
        }


        public static List<Dictionary<string, object>> ExecuteReader(this hotelsContext context, string command, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, int? commandTimeOutInSeconds = null) {
            try {
                using (var cmd = context.Database.GetDbConnection().CreateCommand()) {
                    if (cmd.Connection?.State != ConnectionState.Open) {
                        cmd.Connection?.Open();
                    }
                    var currentTransaction = context.Database.CurrentTransaction;
                    if (currentTransaction != null) {
                        cmd.Transaction = currentTransaction.GetDbTransaction();
                    }
                    cmd.CommandText = command;
                    cmd.CommandType = commandType;
                    if (commandTimeOutInSeconds != null) {
                        cmd.CommandTimeout = (int)commandTimeOutInSeconds;
                    }
                    if (parameters != null) { cmd.Parameters.AddRange(parameters.ToArray()); }

                    DataTable? table = new DataTable();
                    table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                    table.Load(cmd.ExecuteReader());
                    table = (table.DefaultView.Table?.AsDataView()?.Table);

                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in table.Rows) {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in table.Columns) { row.Add(col.ColumnName, dr[col]); }
                        rows.Add(row);
                    }
                    cmd.Connection?.Close();
                    return rows;
                }
            } catch (Exception Ex) { CoreOperations.SendEmail(new SendMailRequest() { Content = DataOperations.GetSystemErrMessage(Ex) }); }
            return null;
        }


        public static IQueryable<TSource> FromSqlRaw<TSource>(this hotelsContext db, string sql, params object[] parameters) where TSource : class {
            var item = db.Set<TSource>().FromSqlRaw(sql, parameters);
            return item;
        }

    }
}