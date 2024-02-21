namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("SolutionMixedEnumList")]
    public class SolutionMixedEnumListApi : ControllerBase {

        [HttpGet("/SolutionMixedEnumList")]
        public async Task<string> GetSolutionMixedEnumList() {
            List<SolutionMixedEnumList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SolutionMixedEnumLists.ToList();
            }
            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionMixedEnumList/Filter/{filter}")]
        public async Task<string> GetSolutionMixedEnumListByFilter(string filter) {
            List<SolutionMixedEnumList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().SolutionMixedEnumLists.FromSqlRaw("SELECT * FROM SolutionMixedEnumList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionMixedEnumList/ByGroup/{groupname}")]
        public async Task<string> GetSolutionMixedEnumListByGroup(string groupname) {
            List<SolutionMixedEnumList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().SolutionMixedEnumLists.Where(a => a.ItemsGroup.ToLower() == groupname.ToLower()).ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/SolutionMixedEnumList/{id}")]
        public async Task<string> GetSolutionMixedEnumListKey(int id) {
            SolutionMixedEnumList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().SolutionMixedEnumLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/SolutionMixedEnumList")]
        [Consumes("application/json")]
        public async Task<string> InsertSolutionMixedEnumList([FromBody] SolutionMixedEnumList record) {
            try {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().SolutionMixedEnumLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/SolutionMixedEnumList")]
        [Consumes("application/json")]
        public async Task<string> UpdateSolutionMixedEnumList([FromBody] SolutionMixedEnumList record) {
            try {
                var data = new hotelsContext().SolutionMixedEnumLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/SolutionMixedEnumList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteSolutionMixedEnumList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                SolutionMixedEnumList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().SolutionMixedEnumLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}