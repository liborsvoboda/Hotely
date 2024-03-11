using System.Text.Json;

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("SystemLoginHistoryList")]
    public class SystemLoginHistoryListApi : ControllerBase {

        [HttpGet("/SystemLoginHistoryList")]
        public async Task<string> GetLoginHistory() {
            List<SystemLoginHistoryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) { data = new hotelsContext().SystemLoginHistoryLists.OrderByDescending(a => a.Timestamp).ToList(); }
            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SystemLoginHistoryList/Filter/{filter}")]
        public async Task<string> GetSystemLoginHistoryListByFilter(string filter) {
            List<SystemLoginHistoryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SystemLoginHistoryLists.FromSqlRaw("SELECT * FROM SystemLoginHistoryList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPost("/SystemLoginHistoryList")]
        [Consumes("application/json")] // ([FromBody] int Id) Body is only 17 ([FromBody] IdFilter id) Body is {"Id":17}
        public async Task<string> GetSystemLoginHistoryListId([FromBody] IdFilter id) {
            if (!int.TryParse(id.Id.ToString(), out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

            SystemLoginHistoryList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().SystemLoginHistoryLists.Where(a => a.Id == id.Id).First(); }
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