namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("EmailerHistoryList")]
    public class EmailerHistoryListApi : ControllerBase {

        [HttpGet("/EmailerHistoryList")]
        public async Task<string> GetEmailerHistoryList() {
            List<EmailerHistoryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().EmailerHistoryLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/EmailerHistoryList/Filter/{filter}")]
        public async Task<string> GetEmailerHistoryListByFilter(string filter) {
            List<EmailerHistoryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().EmailerHistoryLists.FromSqlRaw("SELECT * FROM EmailerHistoryList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/EmailerHistoryList/{id}")]
        public async Task<string> GetEmailerHistoryListKey(int id) {
            EmailerHistoryList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().EmailerHistoryLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/EmailerHistoryList")]
        [Consumes("application/json")]
        public async Task<string> InsertEmailerHistoryList([FromBody] EmailerHistoryList record) {
            try
            {
                var data = new hotelsContext().EmailerHistoryLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/EmailerHistoryList")]
        [Consumes("application/json")]
        public async Task<string> UpdateEmailerHistoryList([FromBody] EmailerHistoryList record) {
            try
            {
                var data = new hotelsContext().EmailerHistoryLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/EmailerHistoryList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteEmailerHistoryList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                EmailerHistoryList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().EmailerHistoryLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) });
            }
        }
    }
}