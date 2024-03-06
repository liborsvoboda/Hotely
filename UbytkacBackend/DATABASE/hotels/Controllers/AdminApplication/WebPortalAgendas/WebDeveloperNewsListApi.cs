namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebDeveloperNewsList")]
    public class WebDeveloperNewsListApi : ControllerBase {

        [HttpGet("/WebDeveloperNewsList")]
        public async Task<string> GetWebDeveloperNewsList() {
            List<WebDeveloperNewsList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().WebDeveloperNewsLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/WebDeveloperNewsList/Filter/{filter}")]
        public async Task<string> GetWebDeveloperNewsListByFilter(string filter) {
            List<WebDeveloperNewsList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().WebDeveloperNewsLists.FromSqlRaw("SELECT * FROM WebDeveloperNewsList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/WebDeveloperNewsList/{id}")]
        public async Task<string> GetWebDeveloperNewsListKey(int id) {
            WebDeveloperNewsList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().WebDeveloperNewsLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/WebDeveloperNewsList")]
        [Consumes("application/json")]
        public async Task<string> InsertWebDeveloperNewsList([FromBody] WebDeveloperNewsList record) {
            try {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().WebDeveloperNewsLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/WebDeveloperNewsList")]
        [Consumes("application/json")]
        public async Task<string> UpdateWebDeveloperNewsList([FromBody] WebDeveloperNewsList record) {
            try {
                var data = new hotelsContext().WebDeveloperNewsLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/WebDeveloperNewsList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteWebDeveloperNewsList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                WebDeveloperNewsList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().WebDeveloperNewsLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}