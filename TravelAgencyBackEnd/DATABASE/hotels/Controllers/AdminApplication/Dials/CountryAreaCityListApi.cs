using UbytkacBackend;

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("CountryAreaCityList")]
    public class CountryAreaCityListApi : ControllerBase {

        [HttpGet("/CountryAreaCityList/{icacId}")]
        public async Task<string> GetCountryAreaCityListKey(int icacId) {
            List<CountryAreaCityList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().CountryAreaCityLists.Where(a => a.Icacid == icacId).ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/CountryAreaCityList")]
        [Consumes("application/json")]
        public async Task<string> InsertAllDocCountryAreaCityList([FromBody] List<CountryAreaCityList> record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    int result;
                    hotelsContext data = new();
                    data.CountryAreaCityLists.AddRange(record);
                    result = data.SaveChanges();

                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpDelete("/CountryAreaCityList/{icacId}")]
        [Consumes("application/json")]
        public async Task<string> DeleteItemList(int icacId) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    List<CountryAreaCityList> data;
                    data = new hotelsContext().CountryAreaCityLists.Where(a => a.Icacid == icacId).ToList();
                    hotelsContext data1 = new();
                    data1.CountryAreaCityLists.RemoveRange(data);
                    int result = data1.SaveChanges();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }
    }
}