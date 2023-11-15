namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebMottoList")]
    public class WebMottoListApi : ControllerBase {

        [HttpGet("/WebMottoList")]
        public async Task<string> GetWebMottoList() {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    List<WebMottoList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().WebMottoLists.ToList(); }
                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpGet("/WebMottoList/Filter/{filter}")]
        public async Task<string> GetWebMottoListByFilter(string filter) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    List<WebMottoList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().WebMottoLists.FromSqlRaw("SELECT * FROM WebMottoList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList(); }
                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpGet("/WebMottoList/{id}")]
        public async Task<string> GetWebMottoListKey(int id) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    WebMottoList data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                    { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().WebMottoLists.Where(a => a.Id == id).First(); }

                    return JsonSerializer.Serialize(data);
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpPut("/WebMottoList")]
        [Consumes("application/json")]
        public async Task<string> InsertWebMottoList([FromBody] WebMottoList record) {
            try
            {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    var data = new hotelsContext().WebMottoLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }


        [HttpPost("/WebMottoList")]
        [Consumes("application/json")]
        public async Task<string> UpdateWebMottoList([FromBody] WebMottoList record) {
            try
            {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    var data = new hotelsContext().WebMottoLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }


        [HttpDelete("/WebMottoList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteWebMottoList(string id) {
            try
            {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    WebMottoList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().WebMottoLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }
    }
}