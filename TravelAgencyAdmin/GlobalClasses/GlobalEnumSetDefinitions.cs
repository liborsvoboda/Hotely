using UbytkacAdmin.Classes;
using System.Collections.ObjectModel;

namespace UbytkacAdmin.GlobalClasses {

    internal class SystemLocalEnumSets {

        /// <summary>
        /// Client Setting Offline enum of Languages
        /// </summary>
        public static ObservableCollection<Language> languages = new ObservableCollection<Language>() {
                                                                new Language() { Name = "System", Value = "system" },
                                                                new Language() { Name =  "czech", Value = "cs-CZ" },
                                                                new Language() { Name = "English", Value = "en-US" }
                                                             };
        /// <summary>
        /// Client Setting offline enum Updater States
        /// </summary>
        public static ObservableCollection<UpdateVariant> updateVariants = new ObservableCollection<UpdateVariant>() {
                                                                new UpdateVariant() { Name = "never", Value = "never" },
                                                                new UpdateVariant() { Name = "showInfo", Value = "showInfo"},
                                                                new UpdateVariant() { Name ="automaticDownload", Value = "automaticDownload"},
                                                                new UpdateVariant() { Name ="automaticInstall", Value = "automaticInstall"}
                                                             };
        /// <summary>
        /// Client Setting offline enum Menu Groups
        /// </summary>
        public static ObservableCollection<UpdateVariant> sections = new ObservableCollection<UpdateVariant>() {
                                                                new UpdateVariant() { Name ="connection", Value = "connection" },
                                                                new UpdateVariant() { Name ="system", Value = "system"},
                                                                new UpdateVariant() { Name ="appearance", Value = "appearance"},
                                                                new UpdateVariant() { Name ="behavior", Value = "behavior"},
                                                             };

        /// <summary>
        /// TODO FOR MOVE TO MICRODIAL
        /// </summary>

        public static ObservableCollection<Language> SpecificationScriptTypes = new ObservableCollection<Language>() {
            new Language() { Name = "Css", Value = "Css" },
            new Language() { Name = "Js", Value = "Js" },
            new Language() { Name = "MinCss", Value = "MinCss" },
            new Language() { Name = "MinJs", Value = "MinJs" },
            new Language() { Name = "Image", Value = "Image" },
            new Language() { Name = "Media", Value = "Media" },
            new Language() { Name = "Package", Value = "Package" },
            new Language() { Name = "CustomFile", Value = "CustomFile" },
        };

        public static ObservableCollection<Language> MenuTypes = new ObservableCollection<Language>() {
            new Language() { Name = "Dial", Value = "Dial" },
            new Language() { Name = "View", Value = "View" },
            new Language() { Name = "Agenda", Value = "Agenda" }
        };

        public static ObservableCollection<Language> ProcessTypes = new ObservableCollection<Language>() {
            new Language() { Name = "EDCservice", Value = "EDCservice" },
            new Language() { Name = "WINcmd", Value = "WINcmd" },
            new Language() { Name = "URL", Value = "URL" }
        };

        public static ObservableCollection<Language> HealthCheckTypes = new ObservableCollection<Language>() {
            new Language() { Name = "driveSize", Value = "driveSize" },
            new Language() { Name = "processMemory", Value = "processMemory" },
            new Language() { Name = "allocatedMemory", Value = "allocatedMemory" },
            new Language() { Name = "folderExist", Value = "folderExist" },

            new Language() { Name = "ping", Value = "ping" },
            new Language() { Name = "tcpPort", Value = "tcpPort" },
            new Language() { Name = "serverUrlPath", Value = "serverUrlPath" },
            new Language() { Name = "urlPath", Value = "urlPath" },

            new Language() { Name = "mssqlConnection", Value = "mssqlConnection" },
            new Language() { Name = "mysqlConnection", Value = "mysqlConnection" },
            new Language() { Name = "oracleConnection", Value = "oracleConnection" },
            new Language() { Name = "postgresConnection", Value = "postgresConnection" },
        };

        public static ObservableCollection<Language> PagePartType = new ObservableCollection<Language>() {
            new Language() { Name = "HeaderPreCss", Value = "HeaderPreCss" },
            new Language() { Name = "HeaderPreScripts", Value = "HeaderPreScripts" },
            new Language() { Name = "HeaderPostScripts", Value = "HeaderPostScripts" },
            new Language() { Name = "Body", Value = "Body" },
            new Language() { Name = "Footer", Value = "Footer" }
        };
    }
}