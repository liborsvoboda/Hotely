namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("SolutionSchedulerProcessList")]
    public class SolutionSchedulerProcessListApi : ControllerBase {


        [HttpGet("/SolutionSchedulerProcessList")]
        public async Task<string> GetSolutionSchedulerProcessList() {
            List<SolutionSchedulerProcessList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) { data = new hotelsContext().SolutionSchedulerProcessLists.OrderByDescending(a => a.TimeStamp).ToList(); }
            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionSchedulerProcessList/{taskId}")]
        public async Task<string> GetSolutionSchedulerProcessListByParent(int taskId ) {
            List<SolutionSchedulerProcessList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) { data = new hotelsContext().SolutionSchedulerProcessLists.Where(a=>a.ScheduledTaskId == taskId).OrderByDescending(a => a.TimeStamp).ToList(); }
            return JsonSerializer.Serialize(data);
        }

        /*
            [HttpPost("/LoginHistory/Name")]
            [Consumes("application/json")]//([FromBody] string Name) Body is only "tester" ([FromBody] NameFilter Name) Body is {"Name":"tester"}
            public async Task<string> GetLoginHistoryName([FromBody] NameFilter Name)
            {
                if (string.IsNullOrWhiteSpace(Name.Name)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is null" });

                List<LoginHistory> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                { IsolationLevel = IsolationLevel.ReadUncommitted }))
                { data = new hotelsContext().LoginHistories.Where(a => a.UserName == Name.Name.ToString()).ToList(); }
                return JsonSerializer.Serialize(data);
            }
        */
    }
}