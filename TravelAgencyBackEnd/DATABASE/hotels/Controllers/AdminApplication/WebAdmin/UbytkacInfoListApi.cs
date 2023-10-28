namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("UbytkacInfoList")]
    public class UbytkacInfoListApi : ControllerBase {

        [HttpGet("/UbytkacInfoList")]
        public async Task<string> GetUbytkacInfoList() {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    List<UbytkacInfoList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                    })) {
                        data = new hotelsContext().UbytkacInfoLists.ToList();
                    }

                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpGet("/UbytkacInfoList/Filter/{filter}")]
        public async Task<string> GetUbytkacInfoListByFilter(string filter) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    List<UbytkacInfoList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                    })) {
                        data = new hotelsContext().UbytkacInfoLists.FromSqlRaw("SELECT * FROM UbytkacInfoList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
                    }

                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpGet("/UbytkacInfoList/{id}")]
        public async Task<string> GetUbytkacInfoListKey(int id) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    UbytkacInfoList data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted
                    })) {
                        data = new hotelsContext().UbytkacInfoLists.Where(a => a.Id == id).First();
                    }

                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpPut("/UbytkacInfoList")]
        [Consumes("application/json")]
        public async Task<string> InsertUbytkacInfoList([FromBody] UbytkacInfoList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    var data = new hotelsContext().UbytkacInfoLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpPost("/UbytkacInfoList")]
        [Consumes("application/json")]
        public async Task<string> UpdateUbytkacInfoList([FromBody] UbytkacInfoList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    var data = new hotelsContext().UbytkacInfoLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpDelete("/UbytkacInfoList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteUbytkacInfoList(string id) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    UbytkacInfoList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().UbytkacInfoLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }
    }
}