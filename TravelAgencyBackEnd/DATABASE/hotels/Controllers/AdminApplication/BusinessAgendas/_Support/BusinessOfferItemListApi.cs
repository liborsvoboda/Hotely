namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("BusinessOfferSupportList")]
    public class BusinessOfferSupportListApi : ControllerBase {

        [HttpGet("/BusinessOfferSupportList/{documentNumber}")]
        public async Task<string> GetBusinessOfferSupportListKey(string documentNumber) {
            List<BusinessOfferSupportList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().BusinessOfferSupportLists.Where(a => a.DocumentNumber == documentNumber).ToList(); }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/BusinessOfferSupportList")]
        [Consumes("application/json")]
        public async Task<string> InsertAllDocBusinessOfferSupportList([FromBody] List<BusinessOfferSupportList> record) {
            try {
                int result;
                hotelsContext data = new hotelsContext(); data.BusinessOfferSupportLists.AddRange(record);
                result = data.SaveChanges();

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/BusinessOfferSupportList/{documentNumber}")]
        [Consumes("application/json")]
        public async Task<string> DeleteItemList(string documentNumber) {
            try {
                List<BusinessOfferSupportList> data;
                data = new hotelsContext().BusinessOfferSupportLists.Where(a => a.DocumentNumber == documentNumber).ToList();
                hotelsContext data1 = new hotelsContext(); data1.BusinessOfferSupportLists.RemoveRange(data);
                int result = data1.SaveChanges();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }
    }
}