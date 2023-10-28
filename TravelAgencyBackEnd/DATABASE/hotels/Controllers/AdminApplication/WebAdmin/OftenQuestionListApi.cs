namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("OftenQuestionList")]
    public class OftenQuestionListApi : ControllerBase {

        [HttpGet("/OftenQuestionList")]
        public async Task<string> GetOftenQuestionList() {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    List<OftenQuestionList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                    })) {
                        data = new hotelsContext().OftenQuestionLists.ToList();
                    }

                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpGet("/OftenQuestionList/Filter/{filter}")]
        public async Task<string> GetOftenQuestionListByFilter(string filter) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    List<OftenQuestionList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                    })) {
                        data = new hotelsContext().OftenQuestionLists.FromSqlRaw("SELECT * FROM OftenQuestionList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
                    }

                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpGet("/OftenQuestionList/{id}")]
        public async Task<string> GetOftenQuestionListKey(int id) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    OftenQuestionList data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted
                    })) {
                        data = new hotelsContext().OftenQuestionLists.Where(a => a.Id == id).First();
                    }

                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpPut("/OftenQuestionList")]
        [Consumes("application/json")]
        public async Task<string> InsertOftenQuestionList([FromBody] OftenQuestionList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    var data = new hotelsContext().OftenQuestionLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpPost("/OftenQuestionList")]
        [Consumes("application/json")]
        public async Task<string> UpdateOftenQuestionList([FromBody] OftenQuestionList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    var data = new hotelsContext().OftenQuestionLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpDelete("/OftenQuestionList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteOftenQuestionList(string id) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    OftenQuestionList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().OftenQuestionLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }
    }
}