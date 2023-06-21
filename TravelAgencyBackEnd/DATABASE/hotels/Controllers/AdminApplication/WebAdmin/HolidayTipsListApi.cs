namespace TravelAgencyBackEnd.Controllers {

    [Authorize]
    [ApiController]
    [Route("HolidayTipsList")]
    public class HolidayTipsListApi : ControllerBase {

        [HttpGet("/HolidayTipsList")]
        public async Task<string> GetHolidayTipsList() {
            List<HolidayTipsList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().HolidayTipsLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/HolidayTipsList/Filter/{filter}")]
        public async Task<string> GetHolidayTipsListByFilter(string filter) {
            List<HolidayTipsList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().HolidayTipsLists.FromSqlRaw("SELECT * FROM HolidayTipsList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/HolidayTipsList/{id}")]
        public async Task<string> GetHolidayTipsListKey(int id) {
            HolidayTipsList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().HolidayTipsLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/HolidayTipsList")]
        [Consumes("application/json")]
        public async Task<string> InsertHolidayTipsList([FromBody] HolidayTipsList record) {
            try
            {
                var data = new hotelsContext().HolidayTipsLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/HolidayTipsList")]
        [Consumes("application/json")]
        public async Task<string> UpdateHolidayTipsList([FromBody] HolidayTipsList record) {
            try
            {
                var data = new hotelsContext().HolidayTipsLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/HolidayTipsList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteHolidayTipsList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = "Id is not set" });

                HolidayTipsList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().HolidayTipsLists.Remove(record);
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