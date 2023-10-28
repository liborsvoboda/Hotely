namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("HotelReservedRoomList")]
    public class HotelReservedRoomListApi : ControllerBase {

        [HttpGet("/HotelReservedRoomList")]
        public async Task<string> GetHotelReservedRoomList() {
            List<HotelReservedRoomList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().HotelReservedRoomLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/HotelReservedRoomList/Filter/{filter}")]
        public async Task<string> GetHotelReservedRoomListByFilter(string filter) {
            List<HotelReservedRoomList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().HotelReservedRoomLists.FromSqlRaw("SELECT * FROM HotelReservedRoomList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/HotelReservedRoomList/{id}")]
        public async Task<string> GetHotelReservedRoomListKey(int id) {
            HotelReservedRoomList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().HotelReservedRoomLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/HotelReservedRoomList")]
        [Consumes("application/json")]
        public async Task<string> InsertHotelReservedRoomList([FromBody] HotelReservedRoomList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin") || Request.HttpContext.User.IsInRole("advertiser")) {
                    var data = new hotelsContext().HotelReservedRoomLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpPost("/HotelReservedRoomList")]
        [Consumes("application/json")]
        public async Task<string> UpdateHotelReservedRoomList([FromBody] HotelReservedRoomList record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin") || Request.HttpContext.User.IsInRole("advertiser")) {
                    var data = new hotelsContext().HotelReservedRoomLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpDelete("/HotelReservedRoomList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteHotelReservedRoomList(string id) {
            try {
                if (Request.HttpContext.User.IsInRole("admin") || Request.HttpContext.User.IsInRole("advertiser")) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    HotelReservedRoomList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().HotelReservedRoomLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }
    }
}