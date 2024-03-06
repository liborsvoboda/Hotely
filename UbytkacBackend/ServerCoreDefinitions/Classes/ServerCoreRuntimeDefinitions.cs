/*
* Server Core Definitions and extensions
* for optimal running in One clean code structure
*/

using FubarDev.FtpServer;
using Quartz;

namespace UbytkacBackend.ServerCoreStructure {

    /// <summary>
    /// The server runtime data.
    /// </summary>
    public partial class ServerRuntimeData {

        /// <summary>
        /// Gets or Sets the startup_path.
        /// </summary>
        internal static string Startup_path { get; set; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Gets or Sets the startup_path.
        /// </summary>
        internal static string WebRoot_path { get; set; } = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ServerConfigSettings.DefaultStaticWebFilesFolder);

        /// <summary>
        /// Gets or Sets the setting_folder.
        /// </summary>
        internal static string Setting_folder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0]);

        /// <summary>
        /// Gets or Sets the configure file.
        /// </summary>
        internal static string UserPath { get; set; } = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0]), "Users");

        /// <summary>
        /// User Web Root Path for Startup and Any missing address Automatic Redirection Value must
        /// start with / Its Portal Model Central Point
        /// </summary>
        public string SpecialUserWebRootPath { get; set; } = "/Index";

        /// <summary>
        /// Gets or Sets the configure file.
        /// </summary>
        internal static string ConfigFile { get; set; } = "config.json";

        /// <summary>
        /// Gets or Sets the data path.
        /// </summary>
        internal static string DataPath { get; set; } = "Data";

        /// <summary>
        /// Gets or Sets the debug mode.
        /// </summary>
        internal static bool DebugMode { get; set; } = "Development" == Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        /// <summary>
        /// The local db table list.
        /// </summary>
        internal static List<object> LocalDBTableList = new();

        /// <summary>
        /// Cancellation Token for Server Remote Control
        /// </summary>
        internal static CancellationTokenSource ServerCancelToken = new CancellationTokenSource();

        /// <summary>
        /// Server Starup Args
        /// </summary>
        internal static string[] ServerArgs;

        /// <summary>
        /// Server Core Status
        /// </summary>
        internal static string ServerCoreStatus = ServerStatuses.Stopped.ToString();

        /// <summary>
        /// Server Restart Request Indicator
        /// </summary>
        internal static bool ServerRestartRequest;

        /// <summary>
        /// Server Securited FTP Provider for Remote Control
        /// </summary>
        internal static IFtpServerHost? ServerFTPProvider;

        /// <summary>
        /// Server Securited FTP Provider for Remote Control
        /// </summary>
        internal static IScheduler? ServerAutoSchedulerProvider;

        /// <summary>
        /// Central WebSocket List for Easy one place Using
        /// </summary>
        internal static List<Tuple<WebSocket, WebSocketExtension>> CentralWebSocketList = new();

        /// <summary>
        /// Central List With references on Hested Server Sevices For Acess and Using Hosted
        /// Functionalities for example: Start/Stop Service And More Other sub services for Optimal Hosting
        /// </summary>
        internal static ServiceProvider ServerServiceProvider;



        /// <summary>
        /// SercerCore FilesLibrary For Rotator
        /// TODO Clean
        /// </summary>
        internal static Dictionary<object,object> FileRotatorRuntineLibrary = new();


        //Generic Complicated Example
        //internal static List<Tuple<string, T>>? ServerHostedServicesContollerList = new List<Tuple<string, T>>();
    }
}