namespace TravelAgencyBackEnd.Controllers {

    [Authorize]
    [ApiController]
    [Route("RegistrationInfoList")]
    public class RegistrationInfoListApi : ControllerBase {

        [HttpGet("/RegistrationInfoList")]
        public async Task<string> GetRegistrationInfoList() {
            List<RegistrationInfoList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().RegistrationInfoLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/RegistrationInfoList/Filter/{filter}")]
        public async Task<string> GetRegistrationInfoListByFilter(string filter) {
            List<RegistrationInfoList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().RegistrationInfoLists.FromSqlRaw("SELECT * FROM RegistrationInfoList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/RegistrationInfoList/{id}")]
        public async Task<string> GetRegistrationInfoListKey(int id) {
            RegistrationInfoList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().RegistrationInfoLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/RegistrationInfoList")]
        [Consumes("application/json")]
        public async Task<string> InsertRegistrationInfoList([FromBody] RegistrationInfoList record) {
            try
            {
                var data = new hotelsContext().RegistrationInfoLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/RegistrationInfoList")]
        [Consumes("application/json")]
        public async Task<string> UpdateRegistrationInfoList([FromBody] RegistrationInfoList record) {
            try
            {
                var data = new hotelsContext().RegistrationInfoLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/RegistrationInfoList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteRegistrationInfoList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                RegistrationInfoList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().RegistrationInfoLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }
    }
}