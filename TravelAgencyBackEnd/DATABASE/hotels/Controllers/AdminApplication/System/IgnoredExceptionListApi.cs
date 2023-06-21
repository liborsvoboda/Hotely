namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("IgnoredExceptionList")]
    public class IgnoredExceptionListApi : ControllerBase {

        [HttpGet("/IgnoredExceptionList")]
        public async Task<string> GetIgnoredExceptionList() {
            List<IgnoredExceptionList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().IgnoredExceptionLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/IgnoredExceptionList/Filter/{filter}")]
        public async Task<string> GetIgnoredExceptionListByFilter(string filter) {
            List<IgnoredExceptionList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().IgnoredExceptionLists.FromSqlRaw("SELECT * FROM IgnoredExceptionList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/IgnoredExceptionList/{id}")]
        public async Task<string> GetIgnoredExceptionListKey(int id) {
            IgnoredExceptionList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().IgnoredExceptionLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [Authorize]
        [HttpPut("/IgnoredExceptionList")]
        [Consumes("application/json")]
        public async Task<string> InsertIgnoredExceptionList([FromBody] IgnoredExceptionList record) {
            try
            {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().IgnoredExceptionLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [Authorize]
        [HttpPost("/IgnoredExceptionList")]
        [Consumes("application/json")]
        public async Task<string> UpdateIgnoredExceptionList([FromBody] IgnoredExceptionList record) {
            try
            {
                var data = new hotelsContext().IgnoredExceptionLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [Authorize]
        [HttpDelete("/IgnoredExceptionList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteIgnoredExceptionList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = "Id is not set" });

                IgnoredExceptionList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().IgnoredExceptionLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }
    }
}