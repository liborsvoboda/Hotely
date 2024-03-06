namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("BusinessCreditNoteSupportList")]
    public class BusinessCreditNoteSupportListApi : ControllerBase {

        [HttpGet("/BusinessCreditNoteSupportList/{documentNumber}")]
        public async Task<string> GetBusinessCreditNoteSupportListKey(string documentNumber) {
            List<BusinessCreditNoteSupportList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().BusinessCreditNoteSupportLists.Where(a => a.DocumentNumber == documentNumber).ToList(); }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/BusinessCreditNoteSupportList")]
        [Consumes("application/json")]
        public async Task<string> InsertAllDocBusinessCreditNoteSupportList([FromBody] List<BusinessCreditNoteSupportList> record) {
            try {
                int result;
                hotelsContext data = new hotelsContext(); data.BusinessCreditNoteSupportLists.AddRange(record);
                result = data.SaveChanges();

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/BusinessCreditNoteSupportList/{documentNumber}")]
        [Consumes("application/json")]
        public async Task<string> DeleteItemList(string documentNumber) {
            try {
                List<BusinessCreditNoteSupportList> data;
                data = new hotelsContext().BusinessCreditNoteSupportLists.Where(a => a.DocumentNumber == documentNumber).ToList();
                hotelsContext data1 = new hotelsContext(); data1.BusinessCreditNoteSupportLists.RemoveRange(data);
                int result = data1.SaveChanges();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }
    }
}