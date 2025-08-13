using EASYTools.CrashReporter;

namespace EasyITSystemCenter.SystemConfiguration {

    /// <summary>
    /// Libreria condivisa
    /// </summary>
    public class CrashReporterSettings {

        /// <summary>
        /// FirstRun
        /// </summary>
        public static ReportCrash _ReportCrash = new ReportCrash()
        {
            FromEmail = App.ServerSetting.Find(a => a.Key == "EmailerSMTPLoginUsername").Value,
            ToEmail = App.ServerSetting.Find(a => a.Key == "ConfigManagerEmailAddress").Value,
            SMTPHost = App.ServerSetting.Find(a => a.Key == "EmailerSMTPServerAddress").Value,
            Port = int.Parse(App.ServerSetting.Find(a => a.Key == "EmailerSMTPPort").Value),
            UserName = App.ServerSetting.Find(a => a.Key == "EmailerSMTPLoginUsername").Value,
            Password = App.ServerSetting.Find(a => a.Key == "EmailerSMTPLoginPassword").Value,
            EnableSSL = bool.Parse(App.ServerSetting.Find(a => a.Key == "EmailerSMTPSslIsEnabled").Value)
        };
    }
}