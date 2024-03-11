namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("SystemSvgIconList")]
    public class SystemSvgIconListApi : ControllerBase {

        [HttpGet("/SystemSvgIconList")]
        public async Task<string> GetSystemSvgIconList() {
            List<SystemSvgIconList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SystemSvgIconLists.OrderBy(a => a.Name).ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SystemSvgIconList/Filter/{filter}")]
        public async Task<string> GetSystemSvgIconListByFilter(string filter) {
            List<SystemSvgIconList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SystemSvgIconLists.FromSqlRaw("SELECT * FROM SystemSvgIconList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().OrderBy(a => a.Name).ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SystemSvgIconList/{iconName}")]
        public async Task<string> GetSystemSvgIconListKey(string iconName) {
            SystemSvgIconList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().SystemSvgIconLists.Where(a => a.Name == iconName).First();
            }

            return JsonSerializer.Serialize(data);
        }
    }
}