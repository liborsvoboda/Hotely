namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("BasicCalendarList")]
    public class BasicCalendarListApi : ControllerBase {

        [HttpGet("/BasicCalendarList/{userId}")]
        public async Task<string> GetBasicCalendarListById(int userId) {
            List<BasicCalendarList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().BasicCalendarLists.Where(a => a.UserId == userId).ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPost("/BasicCalendarList")]
        [Consumes("application/json")]
        public async Task<string> InsertOrUpdateBasicCalendarList([FromBody] BasicCalendarList record) {
            try {
                int result = 0;
                using (var db = new hotelsContext()) {
                    db.Entry(record).State = !db.BasicCalendarLists.Any(a => a.UserId == record.UserId && a.Date == record.Date) ? EntityState.Added : EntityState.Modified;
                    result = await db.SaveChangesAsync();
                }

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }
    }
}