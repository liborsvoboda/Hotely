namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("ServerHealthCheckTaskList")]
    public class ServerHealthCheckTaskListApi : ControllerBase {

        [HttpGet("/ServerHealthCheckTaskList")]
        public async Task<string> GetServerHealthCheckTaskList() {
            List<ServerHealthCheckTaskList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().ServerHealthCheckTaskLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/ServerHealthCheckTaskList/Filter/{filter}")]
        public async Task<string> GetServerHealthCheckTaskListByFilter(string filter) {
            List<ServerHealthCheckTaskList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().ServerHealthCheckTaskLists.FromSqlRaw("SELECT * FROM ServerHealthCheckTaskList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/ServerHealthCheckTaskList/{id}")]
        public async Task<string> GetServerHealthCheckTaskListKey(int id) {
            ServerHealthCheckTaskList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().ServerHealthCheckTaskLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/ServerHealthCheckTaskList")]
        [Consumes("application/json")]
        public async Task<string> InsertServerHealthCheckTaskList([FromBody] ServerHealthCheckTaskList record) {
            try {
                if (CommunicationController.IsAdmin()) {
                    var data = new hotelsContext().ServerHealthCheckTaskLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/ServerHealthCheckTaskList")]
        [Consumes("application/json")]
        public async Task<string> UpdateServerHealthCheckTaskList([FromBody] ServerHealthCheckTaskList record) {
            try {
                if (CommunicationController.IsAdmin()) {
                    var data = new hotelsContext().ServerHealthCheckTaskLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/ServerHealthCheckTaskList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteServerHealthCheckTaskList(string id) {
            try {
                if (CommunicationController.IsAdmin()) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    ServerHealthCheckTaskList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().ServerHealthCheckTaskLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}