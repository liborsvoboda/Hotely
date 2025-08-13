using UbytkacBackend.DBModel;
using UbytkacBackend.WebPages;

namespace UbytkacBackend.Controllers {


    [ApiController]
    [Route("WebApi/WebPages")]
    public class WebDocumentationApi : ControllerBase {

        [HttpGet("/WebApi/WebPages/GetWebDocumentationList/{name}")]
        [Consumes("application/json")]
        public async Task<string> GetWebDocumentationList(string name) {
            string webDocumentation;
            DocSrvDocumentationList data;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    webDocumentation = new hotelsContext().WebPageSettingLists.Where(a => a.Key == "WebDocumentationHtml").FirstOrDefault().Value;
                }

                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().DocSrvDocumentationLists.Where(a=> a.Name.ToLower() == name.ToLower()).FirstOrDefault();
                }

                webDocumentation = webDocumentation.Replace("AUTOCONTENT", data.MdContent);

                return JsonSerializer.Serialize(webDocumentation, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }


      
    }
}