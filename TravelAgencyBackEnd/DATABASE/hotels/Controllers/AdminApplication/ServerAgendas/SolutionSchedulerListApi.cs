namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("SolutionSchedulerList")]
    public class SolutionSchedulerListApi : ControllerBase {

        [HttpGet("/SolutionSchedulerList")]
        public async Task<string> GetSolutionSchedulerList() {
            List<SolutionSchedulerList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SolutionSchedulerLists.ToList();
            }
            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionSchedulerList/Filter/{filter}")]
        public async Task<string> GetSolutionSchedulerListByFilter(string filter) {
            List<SolutionSchedulerList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SolutionSchedulerLists.FromSqlRaw("SELECT * FROM SolutionSchedulerList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionSchedulerList/{id}")]
        public async Task<string> GetSolutionSchedulerListKey(int id) {
            SolutionSchedulerList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().SolutionSchedulerLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/SolutionSchedulerList")]
        [Consumes("application/json")]
        public async Task<string> InsertSolutionSchedulerList([FromBody] SolutionSchedulerList record) {
            try {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().SolutionSchedulerLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                
                await ServerCoreAutoScheduler.AutoSchedulerPlanner(record.Id);
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/SolutionSchedulerList")]
        [Consumes("application/json")]
        public async Task<string> UpdateSolutionSchedulerList([FromBody] SolutionSchedulerList record) {
            try {
                var data = new hotelsContext().SolutionSchedulerLists.Update(record);
                int result = await data.Context.SaveChangesAsync();

                await ServerCoreAutoScheduler.AutoSchedulerPlanner(record.Id);
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/SolutionSchedulerList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteSolutionSchedulerList(int id) {
            try {

                SolutionSchedulerList record = new() { Id = id };
                var data = new hotelsContext().SolutionSchedulerLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();

                await ServerCoreAutoScheduler.AutoSchedulerPlanner(record.Id, true);
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}