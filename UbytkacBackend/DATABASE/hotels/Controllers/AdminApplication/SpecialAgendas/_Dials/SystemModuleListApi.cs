namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("SystemModuleList")]
    public class EasyITCenterSystemModuleListApi : ControllerBase {

        [HttpGet("/SystemModuleList")]
        public async Task<string> GetSystemModuleList() {
            List<SystemModuleList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SystemModuleLists.ToList();
            }
            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SystemModuleList/Filter/{filter}")]
        public async Task<string> GetSystemModuleListByFilter(string filter) {
            List<SystemModuleList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SystemModuleLists.FromSqlRaw("SELECT * FROM SystemModuleList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SystemModuleList/{id}")]
        public async Task<string> GetSystemModuleListKey(int id) {
            SystemModuleList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().SystemModuleLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/SystemModuleList")]
        [Consumes("application/json")]
        public async Task<string> InsertSystemModuleList([FromBody] SystemModuleList record) {
            try {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().SystemModuleLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/SystemModuleList")]
        [Consumes("application/json")]
        public async Task<string> UpdateSystemModuleList([FromBody] SystemModuleList record) {
            try {
                var data = new hotelsContext().SystemModuleLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/SystemModuleList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteSystemModuleList(int id) {
            try {
                SystemModuleList record = new() { Id = id };
                var data = new hotelsContext().SystemModuleLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}