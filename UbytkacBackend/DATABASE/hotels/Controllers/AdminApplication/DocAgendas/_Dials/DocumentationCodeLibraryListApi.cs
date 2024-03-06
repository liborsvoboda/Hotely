namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("DocumentationCodeLibraryList")]
    public class DocumentationCodeLibraryListApi : ControllerBase {

        [HttpGet("/DocumentationCodeLibraryList")]
        public async Task<string> GetDocumentationCodeLibraryList() {
            List<DocumentationCodeLibraryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().DocumentationCodeLibraryLists
                    .OrderBy(a=>a.Name)
                    .ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/DocumentationCodeLibraryList/Filter/{filter}")]
        public async Task<string> GetDocumentationCodeLibraryListByFilter(string filter) {
            List<DocumentationCodeLibraryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().DocumentationCodeLibraryLists.FromSqlRaw("SELECT * FROM DocumentationCodeLibraryList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/DocumentationCodeLibraryList/{id}")]
        public async Task<string> GetDocumentationCodeLibraryListKey(int id) {
            DocumentationCodeLibraryList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().DocumentationCodeLibraryLists.Where(a => a.Id == id).FirstOrDefault();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }


        [HttpPut("/DocumentationCodeLibraryList")]
        [Consumes("application/json")]
        public async Task<string> InsertDocumentationCodeLibraryList([FromBody] DocumentationCodeLibraryList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin".ToLower())) {
                    record.MdContent = DataOperations.MarkDownLineEndSpacesResolve(record.MdContent);
                    var data = new hotelsContext().DocumentationCodeLibraryLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpPost("/DocumentationCodeLibraryList")]
        [Consumes("application/json")]
        public async Task<string> UpdateDocumentationCodeLibraryList([FromBody] DocumentationCodeLibraryList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin".ToLower())) {
					record.MdContent = DataOperations.MarkDownLineEndSpacesResolve(record.MdContent);
                    var data = new hotelsContext().DocumentationCodeLibraryLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpDelete("/DocumentationCodeLibraryList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteDocumentationCodeLibraryList(string id) {
            try {
                if (Request.HttpContext.User.IsInRole("admin".ToLower())) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    DocumentationCodeLibraryList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().DocumentationCodeLibraryLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }
    }
}