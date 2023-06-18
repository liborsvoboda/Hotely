using TravelAgencyBackEnd;

namespace EASYDATACenter.Controllers {

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
                int result;
                hotelsContext data = new hotelsContext(); data.InterestAreaCityLists.AddRange(record);
                result = data.SaveChanges();

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = 0, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/InterestAreaCityList/{iacId}")]
        [Consumes("application/json")]
        public async Task<string> DeleteItemList(int iacId) {
            try {
                List<InterestAreaCityList> data;
                data = new hotelsContext().InterestAreaCityLists.Where(a => a.Iacid == iacId).ToList();
                hotelsContext data1 = new hotelsContext(); data1.InterestAreaCityLists.RemoveRange(data);
                int result = data1.SaveChanges();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = 0, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }
    }
}