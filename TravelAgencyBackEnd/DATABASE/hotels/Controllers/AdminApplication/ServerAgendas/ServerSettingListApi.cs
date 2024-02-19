namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("UbytkacBackendServerSettingList")]
    public class UbytkacBackendServerSettingListApi : ControllerBase {

        [HttpGet("/UbytkacBackendServerSettingList")]
        public async Task<string> GetServerSettingList() {
            List<ServerSettingList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().ServerSettingLists.OrderBy(a => a.Type).ThenBy(a => a.Key).ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/UbytkacBackendServerSettingList/{key}")]
        public async Task<string> GetServerSettingListKey(string key) {
            ServerSettingList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().ServerSettingLists.Where(a => a.Key == key).First();
            }
            return JsonSerializer.Serialize(data);
        }

        [Authorize]
        [HttpPost("/UbytkacBackendServerSettingList")]
        [Consumes("application/json")]
        public async Task<string> SetServerSettingListUpdate([FromBody] List<ServerSettingList> record) {
            try {
                if (CommunicationController.IsAdmin()) {
                    int count = 0;
                    foreach (ServerSettingList item in record) {
                        var data = new hotelsContext().ServerSettingLists.Update(item);
                        int result = await data.Context.SaveChangesAsync();
                        count += result;
                    }
                    if (count > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = count, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = count, ErrorMessage = string.Empty });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}