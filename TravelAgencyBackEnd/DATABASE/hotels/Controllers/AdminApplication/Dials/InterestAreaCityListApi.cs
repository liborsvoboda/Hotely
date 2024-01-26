

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("InterestAreaCityList")]
    public class InterestAreaCityListApi : ControllerBase {

        [HttpGet("/InterestAreaCityList/{iacId}")]
        public async Task<string> GetInterestAreaCityListKey(int iacId) {
            List<InterestAreaCityList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().InterestAreaCityLists.Where(a => a.Iacid == iacId).ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/InterestAreaCityList")]
        [Consumes("application/json")]
        public async Task<string> InsertAllDocInterestAreaCityList([FromBody] List<InterestAreaCityList> record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    int result;
                    hotelsContext data = new hotelsContext(); data.InterestAreaCityLists.AddRange(record);
                    result = data.SaveChanges();

                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpDelete("/InterestAreaCityList/{iacId}")]
        [Consumes("application/json")]
        public async Task<string> DeleteItemList(int iacId) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    List<InterestAreaCityList> data;
                    data = new hotelsContext().InterestAreaCityLists.Where(a => a.Iacid == iacId).ToList();
                    hotelsContext data1 = new hotelsContext(); data1.InterestAreaCityLists.RemoveRange(data);
                    int result = data1.SaveChanges();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }
    }
}