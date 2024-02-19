namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("UbytkacBackendDocSrvDocTemplateList")]
    public class UbytkacBackendDocSrvDocTemplateListApi : ControllerBase {

        [HttpGet("/UbytkacBackendDocSrvDocTemplateList")]
        public async Task<string> GetDocSrvDocTemplateList() {
            List<DocSrvDocTemplateList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().DocSrvDocTemplateLists.OrderBy(a => a.Sequence).ThenBy(a => a.Name).ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/UbytkacBackendDocSrvDocTemplateList/Filter/{filter}")]
        public async Task<string> GetDocSrvDocTemplateListByFilter(string filter) {
            List<DocSrvDocTemplateList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().DocSrvDocTemplateLists.FromSqlRaw("SELECT * FROM DocSrvDocTemplateList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/UbytkacBackendDocSrvDocTemplateList/{id}")]
        public async Task<string> GetDocSrvDocTemplateListKey(int id) {
            DocSrvDocTemplateList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().DocSrvDocTemplateLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/UbytkacBackendDocSrvDocTemplateList")]
        [Consumes("application/json")]
        public async Task<string> InsertDocSrvDocTemplateList([FromBody] DocSrvDocTemplateList record) {
            try {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().DocSrvDocTemplateLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/UbytkacBackendDocSrvDocTemplateList")]
        [Consumes("application/json")]
        public async Task<string> UpdateDocSrvDocTemplateList([FromBody] DocSrvDocTemplateList record) {
            try {
                var data = new hotelsContext().DocSrvDocTemplateLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/UbytkacBackendDocSrvDocTemplateList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteDocSrvDocTemplateList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                DocSrvDocTemplateList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().DocSrvDocTemplateLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}