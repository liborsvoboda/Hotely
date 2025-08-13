namespace UbytkacBackend.DBModel {


    /// <summary>
    /// Database response types definition
    /// </summary>
    public enum DBResult {
        success,
        error,
        DeniedYouAreNotAdmin,
        UnauthorizedRequest
    }

    /// <summary>
    /// The DB result message.
    /// </summary>
    public class DBResultMessage {

        /// <summary>
        /// Gets or Sets the inserted id.
        /// </summary>
        public int InsertedId { get; set; } = 0;

        /// <summary>
        /// Gets or Sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or Sets the record count.
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// Gets or Sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// The authenticate response.
    /// </summary>
    public class AuthenticateResponse {

        /// <summary>
        /// Gets or Sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or Sets the token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or Sets the expiration.
        /// </summary>
        public DateTime? Expiration { get; set; }

        /// <summary>
        /// Gets or Sets the role.
        /// </summary>
        public string Role { get; set; }
    }


    /// <summary>
    /// Database Model Extension Definitions Its API Filter, Extended Classes, Translation, etc
    /// </summary>
    public class SetReportFilter {
        public string TableName { get; set; } = null;
        public string Filter { get; set; } = null;
        public string Search { get; set; } = null;
        public int RecId { get; set; } = 0;
    }

    /// <summary>
    /// Custom Class Definition for Filtering by record Id
    /// </summary>
    public class IdFilter {
        public int Id { get; set; }
    }

    /// <summary>
    /// Custom Class Definition for Filtering by string
    /// </summary>
    public class NameFilter {
        public string Name { get; set; }
    }


    /// <summary>
    /// Custom Definition for Returning string List from Stored Procedures Named Data = ColumnName
    /// in the Data string Can be Any Object
    /// </summary>
    public class GenericDataList {
        public string Data { get; set; }
    }


    /// <summary>
    /// Custom Definition for Returning string List from Stored Procedures Name is ColumnName from
    /// Stored Procedure Result
    /// </summary>
    public class CustomString {
        public string TableList { get; set; }
    }

    /// <summary>
    /// Custom Definition for Returning List with One Record from Operation Stored Procedures
    /// </summary>
    public class SystemOperationMessage {
        public string MessageList { get; set; }
    }

    /// <summary>
    /// Generic Table Snadard Fileds Public Class For Get Informations By System
    /// </summary>
    public class GenericTable {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
    }


    public abstract class GenericModel {
        public Guid Id { get; set; } = Guid.NewGuid();
    }


    public partial class SpUserMenuList {
        public int Id { get; set; }
        public string MenuType { get; set; } = null!;
        public int GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public string FormPageName { get; set; } = null!;
        public string AccessRole { get; set; } = null!;
        public string? Description { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public partial class DBJsonFile {
        public string Value { get; set; } = null!;
    }


    public class SimpleImageList {
        public int Id { get; set; }
        public bool IsPrimary { get; set; }
        public string FileName { get; set; }
    }
}