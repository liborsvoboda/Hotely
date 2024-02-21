namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("BusinessOutgoingOrderSupportList")]
    public class BusinessOutgoingOrderSupportListApi : ControllerBase {

        [HttpGet("/BusinessOutgoingOrderSupportList/{documentNumber}")]
        public async Task<string> GetBusinessOutgoingOrderSupportListKey(string documentNumber) {
            List<BusinessOutgoingOrderSupportList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().BusinessOutgoingOrderSupportLists.Where(a => a.DocumentNumber == documentNumber).ToList(); }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/BusinessOutgoingOrderSupportList")]
        [Consumes("application/json")]
        public async Task<string> InsertAllDocBusinessOutgoingOrderSupportList([FromBody] List<BusinessOutgoingOrderSupportList> record) {
            try {
                int result;
                hotelsContext data = new hotelsContext(); data.BusinessOutgoingOrderSupportLists.AddRange(record);
                result = data.SaveChanges();

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/BusinessOutgoingOrderSupportList/{documentNumber}")]
        [Consumes("application/json")]
        public async Task<string> DeleteItemList(string documentNumber) {
            try {
                List<BusinessOutgoingOrderSupportList> data;
                data = new hotelsContext().BusinessOutgoingOrderSupportLists.Where(a => a.DocumentNumber == documentNumber).ToList();
                hotelsContext data1 = new hotelsContext(); data1.BusinessOutgoingOrderSupportLists.RemoveRange(data);
                int result = data1.SaveChanges();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }
    }
}