namespace TravelAgencyBackEnd.Controllers {

    [Authorize]
    [ApiController]
    [Route("UbytkacInfoList")]
    public class UbytkacInfoListApi : ControllerBase {

        [HttpGet("/UbytkacInfoList")]
        public async Task<string> GetUbytkacInfoList() {
            List<UbytkacInfoList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().UbytkacInfoLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/UbytkacInfoList/Filter/{filter}")]
        public async Task<string> GetUbytkacInfoListByFilter(string filter) {
            List<UbytkacInfoList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().UbytkacInfoLists.FromSqlRaw("SELECT * FROM UbytkacInfoList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/UbytkacInfoList/{id}")]
        public async Task<string> GetUbytkacInfoListKey(int id) {
            UbytkacInfoList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().UbytkacInfoLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/UbytkacInfoList")]
        [Consumes("application/json")]
        public async Task<string> InsertUbytkacInfoList([FromBody] UbytkacInfoList record) {
            try
            {
                var data = new hotelsContext().UbytkacInfoLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/UbytkacInfoList")]
        [Consumes("application/json")]
        public async Task<string> UpdateUbytkacInfoList([FromBody] UbytkacInfoList record) {
            try
            {
                var data = new hotelsContext().UbytkacInfoLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/UbytkacInfoList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteUbytkacInfoList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = "Id is not set" });

                UbytkacInfoList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().UbytkacInfoLists.Remove(record);
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