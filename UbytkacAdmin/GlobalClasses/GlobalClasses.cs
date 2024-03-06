using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace EasyITSystemCenter.GlobalClasses {

    /// <summary>
    /// !!!SYSTEM Global Runtime Monitor Definition For One Point monitoring For Processes and each
    /// other Definition For optimize the System Running
    ///
    /// TODO
    /// - move All Central Definitions Here
    /// - create Monitor Window for managing
    /// </summary>
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

    /// <summary>
    /// !!SYSTEM Global Definition for System Statuses
    ///
    /// SYSTEM Running mode In debug mode is disabled the System Logger Visual Studio Debugger
    /// difficult operation has problem If you want you can enable SystemLogger by change to: DebugWithSystemLogger
    ///
    /// Its Used as String EveryWhere Its good Soution for Centarized Statuses of System Errors Are
    /// Saved In SystemLogger or Database
    /// </summary>
    public enum SystemStatuses {

        //Mode
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

    //public class Parameter {
    //    public string Value { get; set; } = string.Empty;
    //    public bool Correct { get; set; } = false;
    //}

    /// <summary>
    /// Tilt Document Types Definitions
    /// </summary>
    public enum TiltTargets {
        None,
        InvoiceToCredit,
        InvoiceToReceipt,
        OfferToOrder,
        OrderToInvoice,
        OfferToInvoice,

        ShowCredit,
        ShowReceipt
    }

    /// <summary>
    /// Univessal Document List (Item) for Offer,Order,Invoice
    /// </summary>
    public partial class DocumentItemList {
        public int Id { get; set; } = 0;
        public string DocumentNumber { get; set; } = null;

        public string PartNumber { get; set; } = null;
        public string Name { get; set; } = null;
        public string Unit { get; set; } = null;
        public decimal PcsPrice { get; set; } = 0;
        public decimal Count { get; set; } = 1;
        public decimal TotalPrice { get; set; }
        public decimal Vat { get; set; }
        public decimal TotalPriceWithVat { get; set; }

        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    /// <summary>
    /// Class for Using as customized list the List of API urls for Central using in the system One
    /// Api is One: Dataview / Right / Report Posibility / Menu Item / Page Exist rules for
    /// automatic processing in System Core Logic for simple Developing
    /// </summary>
    public partial class SystemTranslatedTableList {
        public string TableName { get; set; }
        public string Translate { get; set; }
    }

    /// <summary>
    /// Generated Class TableList from Stored Procedure SystemSpGetTableList
    /// </summary>
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