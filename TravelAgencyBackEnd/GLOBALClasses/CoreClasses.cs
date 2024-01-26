using System.Reflection;

namespace UbytkacBackend.CoreClasses {

    public class ServerConfigSettings {
        public static string SpecialServerServiceName { get; set; } = "Úbytkáč Emailer";
        public static string EmailerServiceEmailAddress { get; set; } = "Libor.Svoboda@GroupWare-Solution.Eu";

        public static string EmailerSMTPLoginUsername { get; set; } = "backendsolution@groupware-solution.eu";
        public static string EmailerSMTPServerAddress { get; set; } = "imap.groupware-solution.eu";
        public static string EmailerSMTPLoginPassword { get; set; } = "Hb@6u4NkmC";
        public static int EmailerSMTPPort { get; set; } = 25;
        public static bool EmailerSMTPSslIsEnabled { get; set; } = false;
        public static string EmailerBusinessEmailAddress { get; set; } = "Libor.Svoboda@GroupWare-Solution.Eu";

        public static string StartupPath { get; set; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    }

    public class MailRequest {
        public string? Sender { get; set; } = null;
        public List<string>? Recipients { get; set; } = null;
        public string? Subject { get; set; } = null;
        public string? Content { get; set; } = null;
    }

    public class AutoGenEmailAddress {
        public string? EmailAddress { get; set; } = null;
        public string? Language { get; set; } = "cz";

    }

    public enum DBResult {
        success,
        error,
        DeniedYouAreNotAdmin
    }

    /// <summary>
    /// Auto Updated LocalDials - in Future Can be setted as Server Configurator
    /// </summary>
    public enum ServerLocalDbDials {
        LanguageList
    }

    public class DBResultMessage {
        public int InsertedId { get; set; } = 0;
        public string Status { get; set; }
        public int RecordCount { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AuthenticateResponse {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }

    public class IdFilter {
        public int Id { get; set; }
    }

    public class NameFilter {
        public string Name { get; set; }
    }

    public class PageLanguage {
        public string Language { get; set; }
    }

    public enum DBWebApiResponses {
        emailExist,
        emailNotExist,
        loginInfoSentToEmail,
        inputDataError,
        saveDataError
    }

    /// <summary>
    /// Server Process class for running external prrocesses
    /// </summary>
    public class ProcessClass {
        public string Command { get; set; }
        public string? WorkingDirectory { get; set; } = null;
        public string? Arguments { get; set; } = null;
        public bool WaitForExit = true;
    }
}