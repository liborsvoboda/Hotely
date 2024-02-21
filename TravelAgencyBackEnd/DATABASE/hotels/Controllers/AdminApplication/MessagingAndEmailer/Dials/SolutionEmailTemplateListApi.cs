namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("SolutionEmailTemplateList")]
    public class SolutionEmailTemplateListApi : ControllerBase {

        [HttpGet("/SolutionEmailTemplateList")]
        public async Task<string> GetSolutionEmailTemplateList() {
            List<SolutionEmailTemplateList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SolutionEmailTemplateLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionEmailTemplateList/Filter/{filter}")]
        public async Task<string> GetSolutionEmailTemplateListByFilter(string filter) {
            List<SolutionEmailTemplateList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SolutionEmailTemplateLists.FromSqlRaw("SELECT * FROM SolutionEmailTemplateList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionEmailTemplateList/{id}")]
        public async Task<string> GetSolutionEmailTemplateListKey(int id) {
            SolutionEmailTemplateList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().SolutionEmailTemplateLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/SolutionEmailTemplateList")]
        [Consumes("application/json")]
        public async Task<string> InsertSolutionEmailTemplateList([FromBody] SolutionEmailTemplateList record) {
            try {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().SolutionEmailTemplateLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/SolutionEmailTemplateList")]
        [Consumes("application/json")]
        public async Task<string> UpdateSolutionEmailTemplateList([FromBody] SolutionEmailTemplateList record) {
            try {
                var data = new hotelsContext().SolutionEmailTemplateLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/SolutionEmailTemplateList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteSolutionEmailTemplateList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                SolutionEmailTemplateList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().SolutionEmailTemplateLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}