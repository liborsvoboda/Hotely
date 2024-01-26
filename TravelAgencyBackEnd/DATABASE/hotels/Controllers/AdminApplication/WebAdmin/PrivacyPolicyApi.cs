namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("PrivacyPolicyList")]
    public class PrivacyPolicyListApi : ControllerBase {

        [HttpGet("/PrivacyPolicyList")]
        public async Task<string> GetPrivacyPolicyList() {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    List<PrivacyPolicyList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                    })) {
                        data = new hotelsContext().PrivacyPolicyLists.ToList();
                    }

                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpGet("/PrivacyPolicyList/Filter/{filter}")]
        public async Task<string> GetPrivacyPolicyListByFilter(string filter) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    List<PrivacyPolicyList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                    })) {
                        data = new hotelsContext().PrivacyPolicyLists.FromSqlRaw("SELECT * FROM PrivacyPolicyList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
                    }

                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpGet("/PrivacyPolicyList/{id}")]
        public async Task<string> GetPrivacyPolicyListKey(int id) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    PrivacyPolicyList data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted
                    })) {
                        data = new hotelsContext().PrivacyPolicyLists.Where(a => a.Id == id).First();
                    }

                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpPut("/PrivacyPolicyList")]
        [Consumes("application/json")]
        public async Task<string> InsertPrivacyPolicyList([FromBody] PrivacyPolicyList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    var data = new hotelsContext().PrivacyPolicyLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpPost("/PrivacyPolicyList")]
        [Consumes("application/json")]
        public async Task<string> UpdatePrivacyPolicyList([FromBody] PrivacyPolicyList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    var data = new hotelsContext().PrivacyPolicyLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpDelete("/PrivacyPolicyList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeletePrivacyPolicyList(string id) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    PrivacyPolicyList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().PrivacyPolicyLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }
    }
}