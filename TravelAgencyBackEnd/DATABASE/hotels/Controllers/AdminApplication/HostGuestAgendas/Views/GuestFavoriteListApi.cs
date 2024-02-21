namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("GuestFavoriteList")]
    public class GuestFavoriteListApi : ControllerBase {

        [HttpGet("/GuestFavoriteList")]
        public async Task<string> GetGuestFavoriteList() {
            List<GuestFavoriteList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().GuestFavoriteLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/GuestFavoriteList/Filter/{filter}")]
        public async Task<string> GetGuestFavoriteListByFilter(string filter) {
            List<GuestFavoriteList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().GuestFavoriteLists.FromSqlRaw("SELECT * FROM GuestFavoriteList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/GuestFavoriteList/{id}")]
        public async Task<string> GetGuestFavoriteListKey(int id) {
            GuestFavoriteList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().GuestFavoriteLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/GuestFavoriteList")]
        [Consumes("application/json")]
        public async Task<string> InsertGuestFavoriteList([FromBody] GuestFavoriteList record) {
            try
            {
                var data = new hotelsContext().GuestFavoriteLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/GuestFavoriteList")]
        [Consumes("application/json")]
        public async Task<string> UpdateGuestFavoriteList([FromBody] GuestFavoriteList record) {
            try
            {
                var data = new hotelsContext().GuestFavoriteLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/GuestFavoriteList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteGuestFavoriteList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                GuestFavoriteList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().GuestFavoriteLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}