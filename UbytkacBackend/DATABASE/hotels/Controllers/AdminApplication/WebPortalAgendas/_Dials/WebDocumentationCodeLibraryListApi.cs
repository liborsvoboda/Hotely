namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebDocumentationCodeLibraryList")]
    public class WebDocumentationCodeLibraryListApi : ControllerBase {

        [HttpGet("/WebDocumentationCodeLibraryList")]
        public async Task<string> GetWebDocumentationCodeLibraryList() {
            List<WebDocumentationCodeLibraryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().WebDocumentationCodeLibraryLists
                    .OrderBy(a => a.Name)
                    .ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/WebDocumentationCodeLibraryList/Filter/{filter}")]
        public async Task<string> GetWebDocumentationCodeLibraryListByFilter(string filter) {
            List<WebDocumentationCodeLibraryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().WebDocumentationCodeLibraryLists.FromSqlRaw("SELECT * FROM WebDocumentationCodeLibraryList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/WebDocumentationCodeLibraryList/{id}")]
        public async Task<string> GetWebDocumentationCodeLibraryListKey(int id) {
            WebDocumentationCodeLibraryList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().WebDocumentationCodeLibraryLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/WebDocumentationCodeLibraryList")]
        [Consumes("application/json")]
        public async Task<string> InsertWebDocumentationCodeLibraryList([FromBody] WebDocumentationCodeLibraryList record) {
            try {
                record.MdContent = DataOperations.MarkDownLineEndSpacesResolve(record.MdContent);
                var data = new hotelsContext().WebDocumentationCodeLibraryLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/WebDocumentationCodeLibraryList")]
        [Consumes("application/json")]
        public async Task<string> UpdateWebDocumentationCodeLibraryList([FromBody] WebDocumentationCodeLibraryList record) {
            try {
                record.MdContent = DataOperations.MarkDownLineEndSpacesResolve(record.MdContent);
                var data = new hotelsContext().WebDocumentationCodeLibraryLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/WebDocumentationCodeLibraryList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteWebDocumentationCodeLibraryList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                WebDocumentationCodeLibraryList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().WebDocumentationCodeLibraryLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}