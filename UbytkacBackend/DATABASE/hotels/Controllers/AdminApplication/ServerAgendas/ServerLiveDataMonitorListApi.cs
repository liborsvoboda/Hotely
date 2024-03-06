namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("ServerLiveDataMonitorList")]
    public class ServerLiveDataMonitorListApi : ControllerBase {

        [HttpGet("/ServerLiveDataMonitorList")]
        public async Task<string> GetServerLiveDataMonitorList() {
            List<ServerLiveDataMonitorList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().ServerLiveDataMonitorLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/ServerLiveDataMonitorList/Filter/{filter}")]
        public async Task<string> GetServerLiveDataMonitorListByFilter(string filter) {
            List<ServerLiveDataMonitorList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().ServerLiveDataMonitorLists.FromSqlRaw("SELECT * FROM ServerLiveDataMonitorList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/ServerLiveDataMonitorList/{id}")]
        public async Task<string> GetServerLiveDataMonitorListKey(int id) {
            ServerLiveDataMonitorList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().ServerLiveDataMonitorLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/ServerLiveDataMonitorList")]
        [Consumes("application/json")]
        public async Task<string> InsertServerLiveDataMonitorList([FromBody] ServerLiveDataMonitorList record) {
            try {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().ServerLiveDataMonitorLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/ServerLiveDataMonitorList")]
        [Consumes("application/json")]
        public async Task<string> UpdateServerLiveDataMonitorList([FromBody] ServerLiveDataMonitorList record) {
            try {
                var data = new hotelsContext().ServerLiveDataMonitorLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/ServerLiveDataMonitorList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteServerLiveDataMonitorList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                ServerLiveDataMonitorList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().ServerLiveDataMonitorLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}