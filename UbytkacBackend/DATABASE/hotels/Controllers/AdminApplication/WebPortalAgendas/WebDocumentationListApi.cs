namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("WebDocumentationList")]
    public class WebDocumentationListApi : ControllerBase {

        [HttpGet("/WebDocumentationList")]
        public async Task<string> GetWebDocumentationList() {
            List<WebDocumentationList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().WebDocumentationLists
                    .OrderBy(a => a.Sequence).ThenBy(a => a.Name)
                    .ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/WebDocumentationList/Filter/{filter}")]
        public async Task<string> GetWebDocumentationListByFilter(string filter) {
            List<WebDocumentationList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().WebDocumentationLists.FromSqlRaw("SELECT * FROM WebDocumentationList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/WebDocumentationList/{id}")]
        public async Task<string> GetWebDocumentationListKey(int id) {
            WebDocumentationList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().WebDocumentationLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [Authorize]
        [HttpPut("/WebDocumentationList")]
        [Consumes("application/json")]
        public async Task<string> InsertWebDocumentationList([FromBody] WebDocumentationList record) {
            try {
                record.MdContent = DataOperations.MarkDownLineEndSpacesResolve(record.MdContent);
                var data = new hotelsContext().WebDocumentationLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [Authorize]
        [HttpPost("/WebDocumentationList")]
        [Consumes("application/json")]
        public async Task<string> UpdateWebDocumentationList([FromBody] WebDocumentationList record) {
            try {
                record.MdContent = DataOperations.MarkDownLineEndSpacesResolve(record.MdContent);
                var data = new hotelsContext().WebDocumentationLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [Authorize]
        [HttpDelete("/WebDocumentationList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteWebDocumentationList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                WebDocumentationList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().WebDocumentationLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}