using System;

namespace GlobalClasses {

    /// <summary>
    /// SYSTEM Running mode
    /// In debug mode is disabled the System Logger
    /// Visual Studio Debugger difficult operation has problem
    /// If you want you can enable SystemLogger by change to: DebugWithSystemLogger
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
    /// Class for Using as customized list the List of API urls
    /// for Central using in the system
    /// One Api is One: Dataview / Right / Report Posibility / Menu Item / Page
    /// Exist rules for automatic processing in System Core Logic for simple Developing
    /// </summary>
    public partial class TranslatedApiList {
        public string ApiTableName { get; set; }
        public string Translate { get; set; }
    }
}