using TravelAgencyBackEnd;

namespace EASYDATACenter.Controllers {

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
                int result;
                hotelsContext data = new(); 
                data.CountryAreaCityLists.AddRange(record);
                result = data.SaveChanges();

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = 0, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/CountryAreaCityList/{icacId}")]
        [Consumes("application/json")]
        public async Task<string> DeleteItemList(int icacId) {
            try {
                List<CountryAreaCityList> data;
                data = new hotelsContext().CountryAreaCityLists.Where(a => a.Icacid == icacId).ToList();
                hotelsContext data1 = new(); 
                data1.CountryAreaCityLists.RemoveRange(data);
                int result = data1.SaveChanges();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = 0, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }
    }
}