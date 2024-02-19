namespace UbytkacBackend.ServerCoreWebPages {

    /// <summary>
    /// Custom Class For Login over Server Web Pages
    /// </summary>
    public class ServerWebPagesLogin {
        public string? Username { get; set; } = null;
        public string? Password { get; set; } = null;

        // public string? Role { get; set; } = null;
    }

    /// <summary>
    /// Server WebPages Communication Token Definition for Security content And Load Allowed Content
    /// </summary>
    public class ServerWebPagesToken {
        public Dictionary<string, string> Data { get; set; }
        public ClaimsPrincipal? UserClaims { get; set; } = null;
        public SecurityToken? Token { get; set; } = null;
        public string? stringToken { get; set; } = null;
        public bool IsValid { get; set; } = false;
    }

    /// <summary>
    /// WebPages User Verification class
    /// </summary>
    public class EmailVerification {
        public string EmailAddress { get; set; } = null;
        public string Language { get; set; } = null;
    }

    /// <summary>
    /// WebPages User Registration class
    /// </summary>
    public class WebRegistration {
        public string EmailAddress { get; set; } = null;
        public string Password { get; set; } = null;
        public string Language { get; set; } = null;
    }

    public class UserProfile {
        public string EmailAddress { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Password { get; set; }
        public string Language { get; set; }
    }

    /// <summary>
    /// Custom Class For UserConfig over Server Web Pages
    /// </summary>
    public class UserConfig {
        public bool UserAutoTranslate { get; set; }
        public bool UserSubscribeNews { get; set; }
    }

    /// <summary>
    /// Custom WebMessage Class For Server Web Pages
    /// </summary>
    public class WebMessage {
        public string Message { get; set; }
    }

    public class WebSettingList1 {
        public List<WebSetting> Settings { get; set; }
    }

    public class WebSetting {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class MinifiedFile {
        public string SpecificationType { get; set; }
        public string FileName { get; set; }
        public string FileContent { get; set; }
    }

    public class WebFileList {
        public List<WebFile> WebFile { get; set; }
    }

    public class WebFile {
        public string WebFileName { get; set; }
        public string? WebFileNamePath { get; set; }
        public string? WebFileContent { get; set; }
    }


    public class WebSystemLogMessage {
        public string? LogLevel { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? ImageName { get; set; }
        public byte[]? Image { get; set; }
        public string? AttachmentName { get; set; }
        public byte[]? Attachment { get; set; }
    }
}