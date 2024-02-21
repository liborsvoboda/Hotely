namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("SolutionMessageTypeList")]
    public class SolutionMessageTypeListApi : ControllerBase {

        [HttpGet("/SolutionMessageTypeList")]
        public async Task<string> GetSolutionMessageTypeList() {
            List<SolutionMessageTypeList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SolutionMessageTypeLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionMessageTypeList/Filter/{filter}")]
        public async Task<string> GetSolutionMessageTypeListByFilter(string filter) {
            List<SolutionMessageTypeList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().SolutionMessageTypeLists.FromSqlRaw("SELECT * FROM SolutionMessageTypeList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionMessageTypeList/{id}")]
        public async Task<string> GetSolutionMessageTypeListKey(int id) {
            SolutionMessageTypeList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().SolutionMessageTypeLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/SolutionMessageTypeList")]
        [Consumes("application/json")]
        public async Task<string> InsertSolutionMessageTypeList([FromBody] SolutionMessageTypeList record) {
            try 
            {
                var data = new hotelsContext().SolutionMessageTypeLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/SolutionMessageTypeList")]
        [Consumes("application/json")]
        public async Task<string> UpdateSolutionMessageTypeList([FromBody] SolutionMessageTypeList record) {
            try
            {
                var data = new hotelsContext().SolutionMessageTypeLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/SolutionMessageTypeList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteSolutionMessageTypeList(int id) {
            try
            {
                SolutionMessageTypeList record = new() { Id = id };

                var data = new hotelsContext().SolutionMessageTypeLists.Remove(record);
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