namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebCoreFileList")]
    public class WebCoreFileListApi : ControllerBase {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public WebCoreFileListApi(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("/WebCoreFileList")]
        public async Task<string> GetWebCoreFileList() {
            List<WebCoreFileList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().WebCoreFileLists
                    .OrderBy(a => a.SpecificationType).ThenBy(a => a.Sequence)
                    .ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/WebCoreFileList/Filter/{filter}")]
        public async Task<string> GetWebCoreFileListByFilter(string filter) {
            List<WebCoreFileList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().WebCoreFileLists.FromSqlRaw("SELECT * FROM WebCoreFileList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/WebCoreFileList/{id}")]
        public async Task<string> GetWebCoreFileListKey(int id) {
            WebCoreFileList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().WebCoreFileLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/WebCoreFileList")]
        [Consumes("application/json")]
        public async Task<string> InsertWebCoreFileList([FromBody] WebCoreFileList record) {
            try {
                if (WebToolsOperations.SaveWebSourceFile(ref _hostingEnvironment, ref record)) {
                    var data = new hotelsContext().WebCoreFileLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();

                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = string.Empty }); }
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/WebCoreFileList")]
        [Consumes("application/json")]
        public async Task<string> UpdateWebCoreFileList([FromBody] WebCoreFileList record) {
            try {
                if (WebToolsOperations.SaveWebSourceFile(ref _hostingEnvironment, ref record)) {
                    var data = new hotelsContext().WebCoreFileLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();

                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = string.Empty }); }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/WebCoreFileList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteWebCoreFileList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                WebCoreFileList origRec = new hotelsContext().WebCoreFileLists.Where(a => a.Id == int.Parse(id)).First();

                if (WebToolsOperations.DeleteWebSourceFile(ref _hostingEnvironment, ref origRec)) {
                    //WebCoreFileList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().WebCoreFileLists.Remove(origRec);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = origRec.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = string.Empty }); }
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}