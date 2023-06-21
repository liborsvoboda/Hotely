namespace TravelAgencyBackEnd.Controllers {

    [Authorize]
    [ApiController]
    [Route("OftenQuestionList")]
    public class OftenQuestionListApi : ControllerBase {

        [HttpGet("/OftenQuestionList")]
        public async Task<string> GetOftenQuestionList() {
            List<OftenQuestionList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().OftenQuestionLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/OftenQuestionList/Filter/{filter}")]
        public async Task<string> GetOftenQuestionListByFilter(string filter) {
            List<OftenQuestionList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().OftenQuestionLists.FromSqlRaw("SELECT * FROM OftenQuestionList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/OftenQuestionList/{id}")]
        public async Task<string> GetOftenQuestionListKey(int id) {
            OftenQuestionList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().OftenQuestionLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/OftenQuestionList")]
        [Consumes("application/json")]
        public async Task<string> InsertOftenQuestionList([FromBody] OftenQuestionList record) {
            try
            {
                var data = new hotelsContext().OftenQuestionLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/OftenQuestionList")]
        [Consumes("application/json")]
        public async Task<string> UpdateOftenQuestionList([FromBody] OftenQuestionList record) {
            try
            {
                var data = new hotelsContext().OftenQuestionLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/OftenQuestionList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteOftenQuestionList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = "Id is not set" });

                OftenQuestionList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().OftenQuestionLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }
    }
}