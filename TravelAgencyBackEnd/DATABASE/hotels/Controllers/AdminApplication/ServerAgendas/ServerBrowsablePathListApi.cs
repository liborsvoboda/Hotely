namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("UbytkacBackendServerBrowsablePathList")]
    public class UbytkacBackendServerBrowsablePathListApi : ControllerBase {

        [HttpGet("/UbytkacBackendServerBrowsablePathList")]
        public async Task<string> GetServerBrowsablePathList() {
            List<ServerBrowsablePathList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().ServerBrowsablePathLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/UbytkacBackendServerBrowsablePathList/Filter/{filter}")]
        public async Task<string> GetServerBrowsablePathListByFilter(string filter) {
            List<ServerBrowsablePathList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().ServerBrowsablePathLists.FromSqlRaw("SELECT * FROM ServerBrowsablePathList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/UbytkacBackendServerBrowsablePathList/{id}")]
        public async Task<string> GetServerBrowsablePathListKey(int id) {
            ServerBrowsablePathList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().ServerBrowsablePathLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/UbytkacBackendServerBrowsablePathList")]
        [Consumes("application/json")]
        public async Task<string> InsertServerBrowsablePathList([FromBody] ServerBrowsablePathList record) {
            try {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().ServerBrowsablePathLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/UbytkacBackendServerBrowsablePathList")]
        [Consumes("application/json")]
        public async Task<string> UpdateServerBrowsablePathList([FromBody] ServerBrowsablePathList record) {
            try {
                var data = new hotelsContext().ServerBrowsablePathLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/UbytkacBackendServerBrowsablePathList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteServerBrowsablePathList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                ServerBrowsablePathList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().ServerBrowsablePathLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}