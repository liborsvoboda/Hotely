namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("HotelApprovalList")]
    public class HotelApprovalListApi : ControllerBase {

        [HttpGet("/HotelApprovalList")]
        public async Task<string> GetHotelApprovalList() {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    List<HotelApprovalList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                    })) {
                        data = new hotelsContext().HotelApprovalLists.ToList();
                    }

                    return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpGet("/HotelApprovalList/Filter/{filter}")]
        public async Task<string> GetHotelApprovalListByFilter(string filter) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    List<HotelApprovalList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                    })) {
                        data = new hotelsContext().HotelApprovalLists.FromSqlRaw("SELECT * FROM HotelApprovalList WHERE 1=1 AND " + filter.Replace("+", " "))
                            .AsNoTracking().ToList();
                    }

                    return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpGet("/HotelApprovalList/{id}")]
        public async Task<string> GetHotelApprovalListKey(int id) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    HotelApprovalList data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted
                    })) {
                        data = new hotelsContext().HotelApprovalLists.Where(a => a.Id == id).First();
                    }

                    return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpGet("/HotelApprovalList/Rooms/{hotelId}")]
        public async Task<string> GetHotelRoomListById(int hotelId) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    List<HotelRoomList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                    })) {
                        data = new hotelsContext().HotelRoomLists.Where(a => a.HotelId == hotelId).AsNoTracking().ToList();
                    }

                    return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpGet("/HotelApprovalList/Properties/{hotelId}")]
        public async Task<string> GetHotelPropertyListById(int hotelId) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    List<HotelPropertyAndServiceList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                    })) {
                        data = new hotelsContext().HotelPropertyAndServiceLists.Where(a => a.HotelId == hotelId).AsNoTracking().ToList();
                    }

                    return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpPost("/HotelApprovalList")]
        [Consumes("application/json")]
        public async Task<string> UpdateHotelApprovalList([FromBody] HotelApprovalList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    var data = new hotelsContext().HotelApprovalLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }
    }
}