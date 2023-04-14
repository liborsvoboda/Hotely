using System;

namespace TravelAgencyAdmin.GlobalClasses
{

    /// <summary>
    /// Server Configuration definition for Backend API  EASYDATACenter
    /// </summary>
    public enum ServerSettingKeys
    {
        ApiPort,
        ServiceEmail,
        SMTPServer,
        SMTPPort,
        SMTPUserName,
        SMTPPassword,
        LoginTimeoutMinutes,
        TimeTokenValidation,
        SocketTimeoutMinutes,
        MaxSocketBufferSizeKb,
        HttpsProtocol,
        CertificateDomain,
        CertificatePassword,
        JwtLocalKey,
        InternalCachingEnabled,
        LoggingCacheTimeMinutes,
        ShowApiDescription
    }

    /// <summary>
    /// Class for User Authentification information 
    /// </summary>
    public class Authentification
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string SurName { get; set; } = string.Empty;
        public string Token { get; set; }
        public string Role { get; set; } = string.Empty;
    }

    /// <summary>
    /// Basic user data for login
    /// </summary>
    public class UserData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Authentification Authentification { get; set; }
    }

    /// <summary>
    /// Global class for using Name/Value - Example Reports, Language and others
    /// </summary>
    public class UpdateVariant
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public enum TiltTargets
    {
        None,
        InvoiceToCredit,
        InvoiceToReceipt,
        OfferToOrder,
        OrderToInvoice,
        OfferToInvoice,

        ShowCredit
    }

    public partial class DocumentItemList
    {
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

    public partial class TranslatedApiList {
        public string ApiTableName { get; set; }
        public string Translate { get; set; }
    }
}



