namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("DocSrvDocumentationList")]
    public class DocSrvDocumentationListApi : ControllerBase {

        [HttpGet("/DocSrvDocumentationList")]
        public async Task<string> GetDocumentationList() {
            List<DocSrvDocumentationList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().DocSrvDocumentationLists
                    .OrderBy(a => a.DocumentationGroup.Sequence).ThenBy(a => a.Sequence).ThenBy(a=>a.Name)
                    .ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/DocSrvDocumentationList/Filter/{filter}")]
        public async Task<string> GetDocumentationListByFilter(string filter) {
            List<DocSrvDocumentationList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().DocSrvDocumentationLists.FromSqlRaw("SELECT * FROM DocSrvDocumentationList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/DocSrvDocumentationList/{id}")]
        public async Task<string> GetDocumentationListKey(int id) {
            DocSrvDocumentationList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().DocSrvDocumentationLists.Where(a => a.Id == id).FirstOrDefault();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }


        [HttpPut("/DocSrvDocumentationList")]
        [Consumes("application/json")]
        public async Task<string> InsertDocumentationList([FromBody] DocSrvDocumentationList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin".ToLower())) {
                    record.MdContent = DataOperations.MarkDownLineEndSpacesResolve(record.MdContent);
                    var data = new hotelsContext().DocSrvDocumentationLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpPost("/DocSrvDocumentationList")]
        [Consumes("application/json")]
        public async Task<string> UpdateDocumentationList([FromBody] DocSrvDocumentationList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin".ToLower())) {
					record.MdContent = DataOperations.MarkDownLineEndSpacesResolve(record.MdContent);
                    var data = new hotelsContext().DocSrvDocumentationLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpDelete("/DocSrvDocumentationList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteDocumentationList(string id) {
            try {
                if (Request.HttpContext.User.IsInRole("admin".ToLower())) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    DocSrvDocumentationList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().DocSrvDocumentationLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }
    }
}