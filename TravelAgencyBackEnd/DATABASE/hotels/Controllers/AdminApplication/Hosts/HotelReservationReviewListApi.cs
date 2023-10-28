namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("HotelReservationReviewList")]
    public class HotelReservationReviewListApi : ControllerBase {

        [HttpGet("/HotelReservationReviewList")]
        public async Task<string> GetHotelReservationReviewList() {
            List<HotelReservationReviewList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().HotelReservationReviewLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/HotelReservationReviewList/Filter/{filter}")]
        public async Task<string> GetHotelReservationReviewListByFilter(string filter) {
            List<HotelReservationReviewList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().HotelReservationReviewLists.FromSqlRaw("SELECT * FROM HotelReservationReviewList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/HotelReservationReviewList/{id}")]
        public async Task<string> GetHotelReservationReviewListKey(int id) {
            HotelReservationReviewList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().HotelReservationReviewLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/HotelReservationReviewList")]
        [Consumes("application/json")]
        public async Task<string> InsertHotelReservationReviewList([FromBody] HotelReservationReviewList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin") || Request.HttpContext.User.IsInRole("Advertiser")) {
                    var data = new hotelsContext().HotelReservationReviewLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpPost("/HotelReservationReviewList")]
        [Consumes("application/json")]
        public async Task<string> UpdateHotelReservationReviewList([FromBody] HotelReservationReviewList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin") || Request.HttpContext.User.IsInRole("Advertiser")) {
                    var data = new hotelsContext().HotelReservationReviewLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }

        [HttpDelete("/HotelReservationReviewList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteHotelReservationReviewList(string id) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin") || Request.HttpContext.User.IsInRole("Advertiser")) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    HotelReservationReviewList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().HotelReservationReviewLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.youNotHaveRight.ToString() });
        }
    }
}