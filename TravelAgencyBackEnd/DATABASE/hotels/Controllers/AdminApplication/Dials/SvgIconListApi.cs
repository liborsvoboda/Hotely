
namespace Golden.Controllers {

    [Authorize]
    [ApiController]
    [Route("SvgIconList")]
    public class SvgIconListApi : ControllerBase {

        [HttpGet("/SvgIconList")]
        public async Task<string> GetSvgIconList() {
            List<SvgIconList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SvgIconLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SvgIconList/Filter/{filter}")]
        public async Task<string> GetSvgIconListByFilter(string filter) {
            List<SvgIconList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SvgIconLists.FromSqlRaw("SELECT * FROM SvgIconList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SvgIconList/{iconName}")]
        public async Task<string> GetSvgIconListKey(string iconName) {
            SvgIconList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().SvgIconLists.Where(a => a.Name == iconName).First();
            }

            return JsonSerializer.Serialize(data);
        }

    }
}