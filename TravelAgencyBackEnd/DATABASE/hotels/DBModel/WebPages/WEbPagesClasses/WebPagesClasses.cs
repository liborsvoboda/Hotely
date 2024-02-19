namespace UbytkacBackend.WebPages {

    public class WebSettingList1 {
        public List<WebSetting> Settings { get; set; }
    }

    public class WebSetting {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public enum DBWebApiResponses {
        emailExist,
        emailNotExist,
        loginInfoSentToEmail,
        inputDataError,
        saveDataError
    }

    public class AutoGenEmailAddress {
        public string? EmailAddress { get; set; } = null;
        public string? Language { get; set; } = "cz";

    }


    public class PageLanguage {
        public string Language { get; set; }
    }
}