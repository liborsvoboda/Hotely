namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("MessageTypeList")]
    public class MessageTypeListApi : ControllerBase {

        [HttpGet("/MessageTypeList")]
        public async Task<string> GetMessageTypeList() {
            List<MessageTypeList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().MessageTypeLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/MessageTypeList/Filter/{filter}")]
        public async Task<string> GetMessageTypeListByFilter(string filter) {
            List<MessageTypeList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().MessageTypeLists.FromSqlRaw("SELECT * FROM MessageTypeList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/MessageTypeList/{id}")]
        public async Task<string> GetMessageTypeListKey(int id) {
            MessageTypeList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().MessageTypeLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/MessageTypeList")]
        [Consumes("application/json")]
        public async Task<string> InsertMessageTypeList([FromBody] MessageTypeList record) {
            try 
            {
                var data = new hotelsContext().MessageTypeLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/MessageTypeList")]
        [Consumes("application/json")]
        public async Task<string> UpdateMessageTypeList([FromBody] MessageTypeList record) {
            try
            {
                var data = new hotelsContext().MessageTypeLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/MessageTypeList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteMessageTypeList(int id) {
            try
            {
                MessageTypeList record = new() { Id = id };

                var data = new hotelsContext().MessageTypeLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) });
            }
        }
    }
}