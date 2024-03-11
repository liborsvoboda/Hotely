namespace UbytkacBackend.ServerCoreDBSettings {

    /// <summary>
    /// Server WebPages Controller
    /// </summary>
    [ApiController]
    [Route("WebApi/WebPages")]
     //[ApiExplorerSettings(IgnoreApi = true)]
    public class WebPagesSystemDataGetOnlyApi : ControllerBase {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public WebPagesSystemDataGetOnlyApi(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("/WebApi/WebPages/GetSettingList")]
        [Consumes("application/json")]
        public async Task<string> GetSettingList() {
            List<WebSettingList> data;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    data = new hotelsContext().WebSettingLists.ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        /// <summary>
        /// Web Developer News Info Api
        /// </summary>
        /// <param name="rec">The record.</param>
        /// <returns></returns>
        [HttpGet("/WebApi/WebPages/GetNewsList")]
        public async Task<string> GetDeveloperNewsList() {
            try {
                List<WebDeveloperNewsList> data = new();
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().WebDeveloperNewsLists.Where(a => a.Active)
                        .OrderByDescending(a => a.TimeStamp).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetSystemErrMessage(ex) }); }
        }

        /// <summary>
        /// MottoList API for Web Portal
        /// </summary>
        /// <returns></returns>
        [HttpGet("/WebApi/WebPages/GetMottoList")]
        public async Task<string> GetMottoList() {
            try {
                List<SolutionMottoList> data = new();
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().SolutionMottoLists.ToList();
                }

                data.ForEach(motto => {
                    motto.SystemName = DbOperations.DBTranslate(motto.SystemName);
                });

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetSystemErrMessage(ex) }); }
        }
    }
}