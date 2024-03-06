namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebBannedIpAddressList")]
    public class WebBannedIpAddressListApi : ControllerBase {

        [HttpGet("/WebBannedIpAddressList")]
        public async Task<string> GetWebBannedIpAddressList() {
            List<WebBannedIpAddressList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().WebBannedIpAddressLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/WebBannedIpAddressList/Filter/{filter}")]
        public async Task<string> GetWebBannedIpAddressListByFilter(string filter) {
            List<WebBannedIpAddressList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().WebBannedIpAddressLists.FromSqlRaw("SELECT * FROM WebBannedIpAddressList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/WebBannedIpAddressList/{id}")]
        public async Task<string> GetWebBannedIpAddressListKey(int id) {
            WebBannedIpAddressList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().WebBannedIpAddressLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/WebBannedIpAddressList")]
        [Consumes("application/json")]
        public async Task<string> InsertWebBannedIpAddressList([FromBody] WebBannedIpAddressList record) {
            try {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().WebBannedIpAddressLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/WebBannedIpAddressList")]
        [Consumes("application/json")]
        public async Task<string> UpdateWebBannedIpAddressList([FromBody] WebBannedIpAddressList record) {
            try {
                var data = new hotelsContext().WebBannedIpAddressLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/WebBannedIpAddressList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteWebBannedIpAddressList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                WebBannedIpAddressList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().WebBannedIpAddressLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}