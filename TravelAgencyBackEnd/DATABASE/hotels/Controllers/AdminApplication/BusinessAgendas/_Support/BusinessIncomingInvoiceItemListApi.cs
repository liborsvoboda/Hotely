namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("BusinessIncomingInvoiceSupportList")]
    public class BusinessIncomingInvoiceSupportListApi : ControllerBase {

        [HttpGet("/BusinessIncomingInvoiceSupportList/{documentNumber}")]
        public async Task<string> GetBusinessIncomingInvoiceSupportListKey(string documentNumber) {
            List<BusinessIncomingInvoiceSupportList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().BusinessIncomingInvoiceSupportLists.Where(a => a.DocumentNumber == documentNumber).ToList(); }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/BusinessIncomingInvoiceSupportList")]
        [Consumes("application/json")]
        public async Task<string> InsertAllDocBusinessIncomingInvoiceSupportList([FromBody] List<BusinessIncomingInvoiceSupportList> record) {
            try {
                int result;
                hotelsContext data = new hotelsContext(); data.BusinessIncomingInvoiceSupportLists.AddRange(record);
                result = data.SaveChanges();

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/BusinessIncomingInvoiceSupportList/{documentNumber}")]
        [Consumes("application/json")]
        public async Task<string> DeleteItemList(string documentNumber) {
            try {
                List<BusinessIncomingInvoiceSupportList> data;
                data = new hotelsContext().BusinessIncomingInvoiceSupportLists.Where(a => a.DocumentNumber == documentNumber).ToList();
                hotelsContext data1 = new hotelsContext(); data1.BusinessIncomingInvoiceSupportLists.RemoveRange(data);
                int result = data1.SaveChanges();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }
    }
}