using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebApi/Advertiser")]
    public class SetReservationDetailShownApi : ControllerBase {

        [HttpGet("/WebApi/Advertiser/SetAdvertiserShown/{id}")]
        public async Task<string> SetAdvertiserShown(int id) {

            List<HotelReservationDetailList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().HotelReservationDetailLists.Where(a=> a.GuestSender && !a.Shown && a.ReservationId == id).ToList();
            }

            int result = 0;
            data.ForEach(async reservationDetail => {
                reservationDetail.Shown = true;
                var data = new hotelsContext().HotelReservationDetailLists.Update(reservationDetail);
                result = await data.Context.SaveChangesAsync();
            });

            if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
        }


        [HttpGet("/WebApi/Guest/SetGuestShown/{id}")]
        public async Task<string> SetGuestShown(int id) {

            List<HotelReservationDetailList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().HotelReservationDetailLists.Where(a => !a.GuestSender && !a.Shown && a.ReservationId == id).ToList();
            }

            int result = 0;
            data.ForEach(async reservationDetail => {
                reservationDetail.Shown = true;
                var data = new hotelsContext().HotelReservationDetailLists.Update(reservationDetail);
                result = await data.Context.SaveChangesAsync();
            });

            if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
        }
    }
}

