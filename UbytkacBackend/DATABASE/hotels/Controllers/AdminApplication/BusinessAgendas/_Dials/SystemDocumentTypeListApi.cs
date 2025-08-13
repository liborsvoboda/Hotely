namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("SystemDocumentTypeList")]
    public class SystemDocumentTypeListApi : ControllerBase {

        [HttpGet("/SystemDocumentTypeList")]
        public async Task<string> GetSystemDocumentTypeList() {
            List<SystemDocumentTypeList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SystemDocumentTypeLists.ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/SystemDocumentTypeList/Filter/{filter}")]
        public async Task<string> GetSystemDocumentTypeListByFilter(string filter) {
            List<SystemDocumentTypeList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SystemDocumentTypeLists.FromSqlRaw("SELECT * FROM SystemDocumentTypeList WHERE 1=1 AND " + filter.Replace("+", " "))
                .AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/SystemDocumentTypeList/{id}")]
        public async Task<string> GetSystemDocumentTypeListKey(int id) {
            SystemDocumentTypeList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().SystemDocumentTypeLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpPut("/SystemDocumentTypeList")]
        [Consumes("application/json")]
        public async Task<string> InsertSystemDocumentTypeList([FromBody] SystemDocumentTypeList record) {
            try {
                if (ServerApiServiceExtension.IsAdmin()) {
                    //Check exist  translations
                    SystemTranslationList languageRec = new hotelsContext().SystemTranslationLists.Where(a => a.SystemName == record.SystemName).FirstOrDefault();

                    //Insert translation before save new Document Type
                    if (languageRec == null) {
                        languageRec = new SystemTranslationList() { SystemName = record.SystemName, UserId = record.UserId, DescriptionCz = record.SystemName, DescriptionEn = record.SystemName };
                        await new hotelsContext().SystemTranslationLists.Add(languageRec).Context.SaveChangesAsync();
                    }

                    record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                    var data = new hotelsContext().SystemDocumentTypeLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/SystemDocumentTypeList")]
        [Consumes("application/json")]
        public async Task<string> UpdateSystemDocumentTypeList([FromBody] SystemDocumentTypeList record) {
            try {
                if (ServerApiServiceExtension.IsAdmin()) {
                    var data = new hotelsContext().SystemDocumentTypeLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/SystemDocumentTypeList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteSystemDocumentTypeList(string id) {
            try {
                if (ServerApiServiceExtension.IsAdmin()) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    SystemDocumentTypeList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().SystemDocumentTypeLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}