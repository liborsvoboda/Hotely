namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("HotelReservationReviewApprovalList")]
    public class HotelReservationReviewApprovalListApi : ControllerBase {

        [HttpGet("/HotelReservationReviewApprovalList")]
        public async Task<string> GetHotelReservationReviewApprovalList() {
            List<HotelReservationReviewList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().HotelReservationReviewLists.Where(a=>a.Approved == null).ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/HotelReservationReviewApprovalList/Filter/{filter}")]
        public async Task<string> GetHotelReservationReviewApprovalListByFilter(string filter) {
            List<HotelReservationReviewList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().HotelReservationReviewLists.FromSqlRaw("SELECT * FROM HotelReservationReviewList WHERE 1=1 AND Approved = null AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/HotelReservationReviewApprovalList/{id}")]
        public async Task<string> GetHotelReservationReviewApprovalListKey(int id) {
            HotelReservationReviewList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().HotelReservationReviewLists.Where(a => a.Id == id && a.Approved == null).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/HotelReservationReviewApprovalList")]
        [Consumes("application/json")]
        public async Task<string> InsertHotelReservationReviewApprovalList([FromBody] HotelReservationReviewList record) {
            try
            {
                var data = new hotelsContext().HotelReservationReviewLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/HotelReservationReviewApprovalList")]
        [Consumes("application/json")]
        public async Task<string> UpdateHotelReservationReviewApprovalList([FromBody] HotelReservationReviewList record) {
            try
            {
                var data = new hotelsContext().HotelReservationReviewLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/HotelReservationReviewApprovalList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteHotelReservationReviewApprovalList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                HotelReservationReviewList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().HotelReservationReviewLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }
    }
}