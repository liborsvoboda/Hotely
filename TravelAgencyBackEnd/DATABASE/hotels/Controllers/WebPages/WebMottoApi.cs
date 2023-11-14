using UbytkacBackend.DBModel;
using UbytkacBackend.WebPages;

namespace UbytkacBackend.Controllers {


    [ApiController]
    [Route("WebApi/WebPages")]
    public class WebMottoApi : ControllerBase {

        [HttpGet("/WebApi/WebPages/GetWebMottoList")]
        [Consumes("application/json")]
        public async Task<string> GetWebMottoList() {
            List<WebMottoList> data;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    data = new hotelsContext().WebMottoLists.Where(a=> a.Active ).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }

    }
}