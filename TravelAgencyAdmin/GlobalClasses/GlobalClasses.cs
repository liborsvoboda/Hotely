using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace GlobalClasses {

    public class AppRuntimeData {
        private static readonly Version version = Assembly.GetExecutingAssembly().GetName().Version;
        private static readonly string appSystemStatuses = Debugger.IsAttached ? SystemStatuses.Debug.ToString() : SystemStatuses.Release.ToString();

        public string startupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public string programDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public string settingFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0]);
        public string reportFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0], "Reports");
        public string updateFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0], "Update");
        public string tempFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0], "Temp");
        public string galleryFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0], "Gallery");
        public string appSettingFile = "config.json";
        public bool webServerRunning = false;


        public string appName = Assembly.GetEntryAssembly().GetName().FullName.Split(',')[0];
        public Version AppVersion { get => version; }
        public string AppSystemStatuses { get => appSystemStatuses; }
        public string appStartupLanguage = Thread.CurrentThread.CurrentCulture.ToString();

        public Dictionary<string, string> AppClientSettings = new Dictionary<string, string>();

    }



    public enum SystemStatuses {

        //Mode
        Debug,

        Release,
        DebugWithSystemLogger
    }



    /// <summary>
    /// SYSTEM Running mode In debug mode is disabled the System Logger Visual Studio Debugger
    /// difficult operation has problem If you want you can enable SystemLogger by change to: DebugWithSystemLogger
    /// </summary>
    public enum RunningMode {
        Debug,
        Release,
        DebugWithSystemLogger
    }

    /// <summary>
    /// Class for User Authentication information
    /// </summary>
    public class Authentification {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string SurName { get; set; } = string.Empty;
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string Role { get; set; }
    }

    /// <summary>
    /// Basic user data for login
    /// </summary>
    public class UserData {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Authentification Authentification { get; set; }
    }

    /// <summary>
    /// Global class for using Name/Value - Example Reports, Language and others
    /// </summary>
    public class UpdateVariant {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Class for Using as customized list the List of API urls for Central using in the system One
    /// Api is One: Dataview / Right / Report Posibility / Menu Item / Page Exist rules for
    /// automatic processing in System Core Logic for simple Developing
    /// </summary>
    public partial class TranslatedApiList {
        public string ApiTableName { get; set; }
        public string Translate { get; set; }
    }


    public partial class SystemTranslatedTableList {
        public string TableName { get; set; }
        public string Translate { get; set; }
    }


    public partial class SpTableList {
        public string TableList { get; set; }
    }

    /// <summary>
    /// Custom Definition for Returning List with One Record from Operation Stored Procedures
    /// </summary>
    public class SystemOperationMessage {
        public string MessageList { get; set; }
    }

    /// <summary>
    /// Custom Definition for Returning List with One Record from Operation Stored Procedures
    /// </summary>
    public class DBJsonFile {
        public string Value { get; set; }
    }

    /// <summary>
    /// Custom Definition for Returning List with One Record from Operation Stored Procedures
    /// </summary>
    public class DeserializedJson {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}