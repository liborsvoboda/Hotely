namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("DocumentAdviceList")]
    public class DocumentAdviceListApi : ControllerBase {

        [HttpGet("/DocumentAdviceList")]
        public async Task<string> GetDocumentAdviceList() {
            List<DocumentAdviceList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                if (Request.HttpContext.User.IsInRole("Admin")) {
                    data = new hotelsContext().DocumentAdviceLists.ToList();
                }
                else {
                    data = new hotelsContext().DocumentAdviceLists.Include(a => a.User)
                        .Where(a => a.User.UserName == Request.HttpContext.User.Claims.First().Issuer).ToList();
                }
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/DocumentAdviceList/Filter/{filter}")]
        public async Task<string> GetDocumentAdviceListByFilter(string filter) {
            List<DocumentAdviceList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                if (Request.HttpContext.User.IsInRole("Admin")) { data = new hotelsContext().DocumentAdviceLists.FromSqlRaw("SELECT * FROM DocumentAdviceList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList(); }
                else {
                    data = new hotelsContext().DocumentAdviceLists.FromSqlRaw("SELECT * FROM DocumentAdviceList WHERE 1=1 AND " + filter.Replace("+", " "))
                        .Include(a => a.User).Where(a => a.User.UserName == Request.HttpContext.User.Claims.First().Issuer)
                        .AsNoTracking().ToList();
                }
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/DocumentAdviceList/{userId}/{documentTypeId}/{branchId}")]
        public async Task<string> GetDocumentAdviceListType(int userId, int documentTypeId, int branchId) {
            DocumentAdviceList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().DocumentAdviceLists.Where(a => a.UserId == userId && a.DocumentTypeId == documentTypeId && a.BranchId == branchId &&
                (a.StartDate == null || a.StartDate <= DateTime.UtcNow.Date) && (a.EndDate == null || a.EndDate >= DateTime.UtcNow.Date)).FirstOrDefault();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpPut("/DocumentAdviceList")]
        [Consumes("application/json")]
        public async Task<string> InsertDocumentAdviceList([FromBody] DocumentAdviceList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin")) {
                    record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                    var data = new hotelsContext().DocumentAdviceLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpPost("/DocumentAdviceList")]
        [Consumes("application/json")]
        public async Task<string> UpdateDocumentAdviceList([FromBody] DocumentAdviceList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin")) {
                    var data = new hotelsContext().DocumentAdviceLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpDelete("/DocumentAdviceList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteDocumentAdviceList(string id) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin")) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    DocumentAdviceList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().DocumentAdviceLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }
    }
}