
namespace TravelAgencyBackEnd.CoreClasses
{
    public enum DBResult
    {
        success,
        error
    }

    /// <summary>
    /// Auto Updated LocalDials - in Future Can be setted as Server Configurator
    /// </summary>
    public enum ServerLocalDbDials {
        LanguageList
    }


    public class DBResultMessage
    {
        public int insertedId { get; set; } = 0;
        public string status { get; set; }
        public int recordCount { get; set; }
        public string message { get; set; }
    }

    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }

    public class IdFilter
    {
        public int Id { get; set; }
    }

    public class NameFilter
    {
        public string Name { get; set; }
    }

    public class PageLanguage
    {
        public string Language { get; set; }
    }


    public enum DBWebApiResponses {

        emailExist,
        loginInfoSendedOnEmail
    }
}
