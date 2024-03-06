namespace UbytkacBackend.ServerCoreStructure {

    /// <summary>
    /// The server settings.
    /// </summary>
    public partial class ServerConfigSettings {

        #region Emailer Service

        /// <summary>
        /// Service email, for info about not available service from HeatchCheck Can be used for
        /// next Develop, automatic checking problems, missing data and all other Its Necessary for
        /// Correct running Server Internal Core Monitoring
        /// </summary>
        public static string EmailerServiceEmailAddress { get; set; } = "Libor.Svoboda@GroupWare-Solution.Eu";

        /// <summary>
        /// SMTP Server Address for Login to External Mail Server Its Necessary for Correct running
        /// Server Internal Core Monitoring
        /// </summary>
        public static string EmailerSMTPServerAddress { get; set; } = "imap.groupware-solution.eu";

        /// <summary>
        /// SMTP Port for Login to External Mail Server Its Necessary for Correct running Server
        /// Internal Core Monitoring
        /// </summary>
        public static int EmailerSMTPPort { get; set; } = 25;

        /// <summary>
        /// EmailerSMTPSslIsEnabled SSl Email Protocol for Login to External Mail Server Its
        /// Necessary for Correct running Server Internal Core Monitoring
        /// </summary>
        public static bool EmailerSMTPSslIsEnabled { get; set; } = false;

        /// <summary>
        /// SMTP UserName for Login to External Mail Server Its Necessary for Correct running Server
        /// Internal Core Monitoring
        /// </summary>
        public static string EmailerSMTPLoginUsername { get; set; } = "backendsolution@groupware-solution.eu";

        /// <summary>
        /// SMTP Password for Login to External Mail Server Its Necessary for Correct running Server
        /// Internal Core Monitoring
        /// </summary>
        public static string EmailerSMTPLoginPassword { get; set; } = "kuK7VzrU@V";

        #endregion Emailer Service

        #region Server Database

        /// <summary>
        /// Server Database Connection string for Full Service of Database
        /// Migration/Api/Check/Unlimited Develop !!!Warning: Check this connection for
        /// Read/Write/Exec is enabled In Config File Must Be Only This Paramneter
        /// </summary>
        public static string DatabaseConnectionString { get; set; } = "Server=127.0.0.1;Database=;Trusted_Connection=True;TrustServerCertificate=True";

        /// <summary>
        /// Enable Disable Entity Framework Internal Cache I recommend turning it off : from Logic,
        /// Duplicit Functionality with Database Server in complex process can generate problems
        /// Recommended: false
        /// </summary>
        public static bool DatabaseInternalCachingEnabled { get; set; } = false;

        /// <summary>
        /// Time in Minutes for Database Valid Cache Data and Refreshing Duplicit Functionality with
        /// Database Server
        /// Recommended: 30
        /// </summary>
        public static int DatabaseInternalCacheTimeoutMin { get; set; } = 30;

        /// <summary>
        /// Enable Logging Server Warn and Errors To Database
        /// </summary>
        public static bool ConfigLogWarnPlusToDbEnabled { get; set; } = false;

        #endregion Server Database

        #region Server Future Technologies

        /// <summary>
        /// Server Service Name automatic figured in All IS/OS/Engines info
        /// Recommended: EASY-IT-CENTER
        /// </summary>
        public static string ConfigCoreServerRegisteredName { get; set; } = "EASY-IT-CENTER";

        /// <summary>
        /// Activation / Deactivation of Email Sender For Server Core Fails Checker All Catch Write
        /// to SendEmail, In Debug mode is Written in console in Release mode is Sended email (All
        /// incorrect server statuses are monitored) Can be writen to Database - !!! Warning for
        /// infinite Cycling (DB shutdown example)
        /// Recommended: true
        /// </summary>
        public static bool ServiceCoreCheckerEmailSenderActive { get; set; } = false;

        /// <summary>
        /// Special Function: Using Selected Tables with AutoRefresh On change as Local DataSets,
        /// For Optimize Traffic. For Example LanguageList - Static table with often reading
        /// Recommended: true - functionality must be implemented in Server Code
        /// </summary>
        public static bool ServiceUseDbLocalAutoupdatedDials { get; set; } = false;

        /// <summary>
        /// Server Language for Translating Server internal statuses
        /// Recommended: cz or en - other languages are not implemented
        /// </summary>
        public static string ServiceServerLanguage { get; set; } = "cz";

        /// <summary>
        /// Server support mass emailing as Service You can enable Mass Email Api
        /// </summary>
        public static bool ServiceEnableMassEmail { get; set; } = true;

        /// <summary>
        /// Enabling Provider APIs More Informations are Published on WepPages Are Published Core
        /// Informations With Full Solutions Info/Tools/Codes/Etc Published
        /// </summary>
        public static bool ServerProviderModeEnabled { get; set; } = true;

        /// <summary>
        /// Server Scheduler Module for Automatization Tasks
        /// </summary>
        /// <value>The serve scheduler enabled.</value>
        public static bool ModuleAutoSchedulerEnabled { get; set; } = true;

        #endregion Server Future Technologies

        #region ServerConfigurationServices

        /// <summary>
        /// Set Server Startup Port on Http/HttpS/WebSocket and for All Engines, Modules, API
        /// Controler and WebPages
        /// Recommended: 5000
        /// </summary>
        public static int ConfigServerStartupHttpPort { get; set; } = 5000;

        /// <summary>
        /// WebSocket Timeout Connection Settings in Minutes. After timeout when not detected any
        /// communication socket is closed Set according to your need
        /// Recommended: 2
        /// </summary>
        public static double WebSocketTimeoutMin { get; set; } = 2;

        /// <summary>
        /// Maximum socket message size for control Traffic
        /// Recomended: 10
        /// </summary>
        public static int WebSocketMaxBufferSizeKb { get; set; } = 10;

        /// <summary>
        /// Bearer Token Timeout Setting in Minutes. Connection must be Refreshed in Interval After
        /// Timeout connection Must Login Again. It is as needed. You Can Change Key for close All
        /// connections Immediately. Timeout is good for Webpages
        /// Recomended: 15
        /// </summary>
        public static double ConfigApiTokenTimeoutMin { get; set; } = 15;

        /// <summary>
        /// Setting for Server URL Services. Logically can run only one Http or Https Server has
        /// more Security Setting Politics.
        /// Recommended: true
        /// </summary>
        public static bool ConfigServerStartupOnHttps { get; set; } = true;

        /// <summary>
        /// Setting for Automatic Obtain Lets Encrypt more Security Setting Politics.
        /// Recommended: true
        /// </summary>
        public static bool ConfigServerGetLetsEncrypt { get; set; } = false;

        /// <summary>
        /// Certificate file NextFrom'DATA'Path\\Filename.pfx For import External Certificate Its
        /// Path from Data Startup Folder,example "groupware.pfx" is in Data Path The Password must
        /// be inserted in ConfigCertificatePassword Settings Other for ignore this Setting set
        /// empty string "" This settings has Higest Priority before LesEncrypt enabled
        /// </summary>
        public static string ConfigCertificatePath { get; set; } = "groupware.pfx";

        /// <summary>
        /// Its Domain for include to Automatic Generated Certificate For Server running on HTTPS.
        /// Domain is for Your validation Certificate Domain. Can be Changed for commercial. Domain
        /// is Used for Lets Encrypt also Write with Comma separator
        /// Recommended: 127.0.0.1
        /// </summary>
        public static string ConfigCertificateDomain { get; set; } = "kliknetezde.cz;kliknetezde.cz:5000";

        /// <summary>
        /// Password will be inserted to Server Generated Certificate for HTTPS.
        /// Recommended: empty = Maximal Security Randomly generated 20 chars string
        /// </summary>
        public static string ConfigCertificatePassword { get; set; } = DataOperations.RandomString(20);

        /// <summary>
        /// Special Functions:Server AutoGenerated encryption Key For Securing Communication On Each
        /// Server Restart All Tokens not will be valid and the Login Reconnect is Requested. Its
        /// AntiHacker Security Rule
        /// Recommended: empty = Maximal Security Randomly generated 40 chars string
        /// </summary>
        public static string ConfigJwtLocalKey { get; set; } = DataOperations.RandomString(40);



        /// <summary>
        /// Startup Server On HTTP and HTTPS on Defined Port 
        /// </summary>
        public static bool ConfigServerStartupHTTPAndHTTPS { get; set; } = false;


        /// <summary>
        /// Set Server Startup  HTTPS Port on Http/HttpS/WebSocket and for All Engines, Modules, API
        ///Controler and WebPages
        ///Recommended: 5001
        /// </summary>
        public static int ConfigServerStartupHttpsPort { get; set; } = 5001;

        #endregion ServerConfigurationServices

        #region Server Engines

        /// <summary>
        /// Enable Disable Bearer Token Timeout Validation Token can be valid EveryTime to using:
        /// Example for machine connection Or is Control last activity
        /// </summary>
        public static bool ConfigTimeTokenValidationEnabled { get; set; } = false;

        /// <summary>
        /// Emailová adresa do obchodních kontaktů
        /// </summary>
        public static string ConfigManagerEmailAddress { get; set; } = "libor.svoboda@kliknetezde.cz";

        /// <summary>
        /// Enable FTP File Server oppened for every connection with full rights
        /// </summary>
        public static bool ServerFtpEngineEnabled { get; set; } = true;

        /// <summary>
        /// Enable FTP Security For access to FTP must be logged
        /// </summary>
        public static bool ServerFtpSecurityEnabled { get; set; } = true;

        /// <summary>
        /// FTP Server Storage Path Definition It Will be under wwwroot for Posibility Share Data or
        /// For WebAccess/Browser Over WebPortal
        /// </summary>
        public static string ServerFtpStorageRootPath { get; set; } = "FTPServer";

        /// <summary>
        /// CORS Hlavčky HTTP Requestů příchozích na server WEB + API, Povolit jakýkoli původ
        /// </summary>
        public static bool ServerCorsAllowAnyOrigin { get; set; } = true;

        /// <summary>
        /// CORS Hlavčky HTTP Requestů příchozích na server WEB + API, Povolit jakoukoliv Metodu
        /// </summary>
        public static bool ServerCorsAllowAnyMethod { get; set; } = true;

        /// <summary>
        /// CORS Hlavčky HTTP Requestů příchozích na server WEB + API, Povolit jakýkoliv Header
        /// </summary>
        public static bool ServerCorsAllowAnyHeader { get; set; } = true;

        /// <summary>
        /// CORS Hlavčky HTTP Requestů příchozích na server WEB + API , Povolit jakékoliv Pověření - Autorizaci
        /// </summary>
        public static bool ServerCorsAllowCredentials { get; set; } = true;


        /// <summary>
        /// Veřejná URL adresa serveru použita v modulech
        /// </summary>
        public static string ServerPublicUrl { get; set; } = "https://KlikneteZde.Cz";
        
        /// <summary>
        /// Server Filtrování Dle CORS Zapnuto
        /// </summary>
        public static bool ServerCorsEnabled { get; set; } = true;



        /// <summary>
        /// Root Složka pro Statické soubory Webu
        /// </summary>
        public static string DefaultStaticWebFilesFolder { get; set; } = "wwwroot";
        #endregion Server Engines

        #region Server Modules

        /// <summary>
        /// Special Function: Server Automatically Generate Full Documentation of API structure AND
        /// Database Model. Plus Included API Interface for Online Testing All API Methods with
        /// Authentication Its Automatic Solution for Third Party cooperation. All changes are
        /// Reflected after restart server
        /// </summary>
        public static bool ModuleSwaggerApiDocEnabled { get; set; } = true;

        /// <summary>
        /// Special Function: More than 200 Server Statuses Can be monitored by HeathCheckService,
        /// Over Net can Control Other Company Services Also as Central Control Point With Email
        /// Service. For Example: Server/Memory/All DB Types/IP/HDD/Port/Api/NET/Cloud/Environment
        /// Must be run for Metrics AddOn https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health
        /// </summary>
        public static bool ModuleHealthServiceEnabled { get; set; } = true;

        /// <summary>
        /// Special Function: More than 200 Server Statuses Can be monitored by HeathCheckService,
        /// Over Net can Control Other Company Services Also as Central Control Point With Email
        /// Service. For Example: Server/Memory/All DB Types/IP/HDD/Port/Api/NET/Cloud/Environment
        /// Must be run for Metrics AddOn !!! run in Release mode for Good Reading of Logs without
        /// HeathChecks Cycling info https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks https://testfully.io/blog/api-health-check-monitoring/
        /// </summary>
        public static int ModuleHealthServiceRefreshIntervalSec { get; set; } = 30;

        /// <summary>
        /// Enable Server Structure Documentation for Developers Using RootPath, - Block File
        /// Browsing Is Good for Server with Documentation only Or Use for Show WebPage and Copy
        /// "Only HTML"
        /// </summary>
        public static bool ModuleMdDocumentationEnabled { get; set; } = true;

        /// <summary>
        /// Enable Automatic Database Diagram for Simple show Database structure with All bingings
        /// </summary>
        public static bool ModuleDbDiagramGeneratorEnabled { get; set; } = true;

        /// <summary>
        /// Api URL cesta Serveru na které bude Dohledové centrum k dispozici
        /// </summary>
        public static string ModuleHealthServicePath { get; set; } = "/ServerHealthService";

        /// <summary>
        /// Api URL cesta Serveru na které bude API Dokumentace a Testovací rozhraní k dispozici
        /// </summary>
        public static string ModuleSwaggerApiDocPath { get; set; } = "AdminApiDocs";

        /// <summary>
        /// Zaznamenávat Logování/zaslání zprávy pouze při změně z OK-&gt;FAIL, jinak bude
        /// zaznamenáno v každém cyklu
        /// </summary>
        public static bool ModuleHealthServiceMessageOnChangeOnly { get; set; } = true;


        /// <summary>
        /// Cestana které se Schema Zobrazí,
        /// default: /DBEntitySchema
        /// </summary>
        public static string ModuleDBEntitySchemaPath { get; set; } = "/DBEntitySchema";
        
        /// <summary>
        /// Modul Zobrazení DB Entitity Schema
        /// </summary>
        public static bool ModuleDBEntitySchemaEnabled { get; set; } = true;



        #endregion Server Modules

        #region Server Web Portal

        /// <summary>
        /// Enable Razor WebPages support engine with Correct Configuration for Automaping from
        /// folder 'ServerCorePages'
        /// </summary>
        public static bool WebRazorPagesEngineEnabled { get; set; } = true;

        /// <summary>
        /// Enable Mvc WebPages support engine with Correct Configuration
        /// </summary>
        public static bool WebMvcPagesEngineEnabled { get; set; } = true;

        /// <summary>
        /// Enable WebSocket Engine with Default Example Api Controller
        /// </summary>
        public static bool WebSocketEngineEnabled { get; set; } = true;

        /// <summary>
        /// Enable WebPages File Browsing from server Url on Server
        /// </summary>
        public static bool WebBrowserContentEnabled { get; set; } = true;

        /// <summary>
        /// Server support online multi watch Logging Its Run on Websocket and the Log Messages are
        /// sent for All opened connections for wathing on Path: ServerCoreMonitor You can enable
        /// Mass Email Api
        /// </summary>
        public static bool WebSocketServerMonitorEnabled { get; set; } = true;

        /// <summary>
        /// Zapnout Generování RSS fédů z položek, novinek
        /// </summary>
        public static bool WebRSSFeedsEnabled { get; set; } = true;

        /// <summary>
        /// zapnout zobrazení souboru Robot.txt, obsah se bere z nastavení Portálu
        /// </summary>
        public static bool WebRobotTxtFileEnabled { get; set; } = true;

        /// <summary>
        /// Generovaný Soubor Sitemap z Menu Portálu
        /// </summary>
        public static bool WebSitemapFileEnabled { get; set; } = true;

        /// <summary>
        /// Module: AutoGenerated Database DataManager for working with Data Running only in Debug
        /// mode for simple Develop, can be modified. All changes are Reflected after restart server
        /// </summary>
        public static bool ModuleWebDataManagerEnabled { get; set; } = true;

        /// <summary>
        /// Enable Server Static Folder Monitor for Data Changes And immediatelly update all Api,
        /// Endpoints, Client Browser
        /// </summary>
        public static bool WebLiveDataMonitorEnabled { get; set; } = true;

        /// <summary>
        /// WebPortal Global Notify Path on WebSocket for sending messages from scheduler
        /// </summary>
        /// <value>The web global socket notify path.</value>
        public static string WebSocketGlobalNotifyPath { get; set; } = "globalnotify";

        /// <summary>
        /// Provádí kompilaci Za běhu Serveru. Tzn.. Podporuje Vkládání Stránek Externě
        /// </summary>
        public static bool WebRazorPagesCompileOnRuntime { get; set; } = false;


        /// <summary>
        /// Aktivuje Presmerovani na Zadanou cestu
        /// v případě Stránka nenalezena
        /// </summary>
        public static bool RedirectOnPageNotFound { get; set; } = true;


        #endregion Server Web Portal

    }
}