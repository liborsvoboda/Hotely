namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("BusinessPaymentMethodList")]
    public class BusinessPaymentMethodListApi : ControllerBase {

        [HttpGet("/BusinessPaymentMethodList")]
        public async Task<string> GetBusinessPaymentMethodList() {
            List<BusinessPaymentMethodList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().BusinessPaymentMethodLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/BusinessPaymentMethodList/Filter/{filter}")]
        public async Task<string> GetBusinessPaymentMethodListByFilter(string filter) {
            List<BusinessPaymentMethodList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().BusinessPaymentMethodLists.FromSqlRaw("SELECT * FROM BusinessPaymentMethodList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/BusinessPaymentMethodList/{id}")]
        public async Task<string> GetBusinessPaymentMethodListKey(int id) {
            BusinessPaymentMethodList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().BusinessPaymentMethodLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/BusinessPaymentMethodList")]
        [Consumes("application/json")]
        public async Task<string> InsertBusinessPaymentMethodList([FromBody] BusinessPaymentMethodList record) {
            try {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().BusinessPaymentMethodLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/BusinessPaymentMethodList")]
        [Consumes("application/json")]
        public async Task<string> UpdateBusinessPaymentMethodList([FromBody] BusinessPaymentMethodList record) {
            try {
                var data = new hotelsContext().BusinessPaymentMethodLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/BusinessPaymentMethodList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteBusinessPaymentMethodList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                BusinessPaymentMethodList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().BusinessPaymentMethodLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}