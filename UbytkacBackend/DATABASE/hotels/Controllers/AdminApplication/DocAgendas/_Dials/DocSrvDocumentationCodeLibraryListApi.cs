namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("DocSrvDocumentationCodeLibraryList")]
    public class DocSrvDocumentationCodeLibraryListApi : ControllerBase {

        [HttpGet("/DocSrvDocumentationCodeLibraryList")]
        public async Task<string> GetDocSrvDocumentationCodeLibraryList() {
            List<DocSrvDocumentationCodeLibraryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().DocSrvDocumentationCodeLibraryLists
                    .OrderBy(a=>a.Name)
                    .ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/DocSrvDocumentationCodeLibraryList/Filter/{filter}")]
        public async Task<string> GetDocSrvDocumentationCodeLibraryListByFilter(string filter) {
            List<DocSrvDocumentationCodeLibraryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().DocSrvDocumentationCodeLibraryLists.FromSqlRaw("SELECT * FROM DocSrvDocumentationCodeLibraryList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/DocSrvDocumentationCodeLibraryList/{id}")]
        public async Task<string> GetDocSrvDocumentationCodeLibraryListKey(int id) {
            DocSrvDocumentationCodeLibraryList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().DocSrvDocumentationCodeLibraryLists.Where(a => a.Id == id).FirstOrDefault();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }


        [HttpPut("/DocSrvDocumentationCodeLibraryList")]
        [Consumes("application/json")]
        public async Task<string> InsertDocSrvDocumentationCodeLibraryList([FromBody] DocSrvDocumentationCodeLibraryList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin".ToLower())) {
                    record.MdContent = DataOperations.MarkDownLineEndSpacesResolve(record.MdContent);
                    var data = new hotelsContext().DocSrvDocumentationCodeLibraryLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpPost("/DocSrvDocumentationCodeLibraryList")]
        [Consumes("application/json")]
        public async Task<string> UpdateDocSrvDocumentationCodeLibraryList([FromBody] DocSrvDocumentationCodeLibraryList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin".ToLower())) {
					record.MdContent = DataOperations.MarkDownLineEndSpacesResolve(record.MdContent);
                    var data = new hotelsContext().DocSrvDocumentationCodeLibraryLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpDelete("/DocSrvDocumentationCodeLibraryList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteDocSrvDocumentationCodeLibraryList(string id) {
            try {
                if (Request.HttpContext.User.IsInRole("admin".ToLower())) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    DocSrvDocumentationCodeLibraryList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().DocSrvDocumentationCodeLibraryLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }
    }
}