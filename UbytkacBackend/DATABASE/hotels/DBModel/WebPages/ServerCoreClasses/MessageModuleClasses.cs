namespace UbytkacBackend.MessageModuleClasses {

    public class WebPrivateMessage {
        public int ParentId { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
    }

    public class WebDiscussionContribution {
        public int ParentId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
    }

}