namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("SolutionEmailerHistoryList")]
    public class SolutionEmailerHistoryListApi : ControllerBase {

        [HttpGet("/SolutionEmailerHistoryList")]
        public async Task<string> GetSolutionEmailerHistoryList() {
            List<SolutionEmailerHistoryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().SolutionEmailerHistoryLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionEmailerHistoryList/Filter/{filter}")]
        public async Task<string> GetSolutionEmailerHistoryListByFilter(string filter) {
            List<SolutionEmailerHistoryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().SolutionEmailerHistoryLists.FromSqlRaw("SELECT * FROM SolutionEmailerHistoryList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionEmailerHistoryList/{id}")]
        public async Task<string> GetSolutionEmailerHistoryListKey(int id) {
            SolutionEmailerHistoryList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().SolutionEmailerHistoryLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/SolutionEmailerHistoryList")]
        [Consumes("application/json")]
        public async Task<string> InsertSolutionEmailerHistoryList([FromBody] SolutionEmailerHistoryList record) {
            try
            {
                var data = new hotelsContext().SolutionEmailerHistoryLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/SolutionEmailerHistoryList")]
        [Consumes("application/json")]
        public async Task<string> UpdateSolutionEmailerHistoryList([FromBody] SolutionEmailerHistoryList record) {
            try
            {
                var data = new hotelsContext().SolutionEmailerHistoryLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/SolutionEmailerHistoryList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteSolutionEmailerHistoryList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                SolutionEmailerHistoryList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().SolutionEmailerHistoryLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}