namespace UbytkacBackend.ServerCoreStructure {

    /// <summary>
    /// Class Definition for Server Emailer In List of this claas you can use Mass Emailer
    /// </summary>
    public class MailRequest {
        public string? Sender { get; set; } = null;
        public List<string>? Recipients { get; set; } = null;
        public string? Subject { get; set; } = null;
        public string? Content { get; set; } = null;
    }

    /// <summary>
    /// Server and Hosted Services Runtime Statusses
    /// </summary>
    public enum ServerStatuses {
        Running,
        Stopping,
        Stopped,
        InStandbyMode
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

    /// <summary>
    /// Special Functions: Definition of Selected tables for Optimal Using to Data nature Its saves
    /// traffic, increases availability and for Example implemented Language is in Develop Auto Fill
    /// Table when is First Using Its can be used for more Dials
    /// </summary>
    public enum ServerLocalDbDials {
        SystemTranslationList,
        ServerModuleAndServiceLists
    }
}