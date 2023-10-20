using System.ComponentModel.Design;
using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("WebApi/Advertiser")]
    public class AdvertiserBookingApi : ControllerBase {

        [Authorize]
        [HttpPost("/WebApi/Advertiser/SetAdvertiserDetail")]
        [Consumes("application/json")]
        public IActionResult SetAdvertiserDetail([FromBody] AdvertiserBookingData record) {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;

                HotelReservationDetailList lastDetail = new HotelReservationDetailList();
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    lastDetail = new hotelsContext().HotelReservationDetailLists.Where(a => a.ReservationId == record.ReservationId)
                        .OrderByDescending(a => a.Id).FirstOrDefault();
                }

                HotelReservationDetailList reservationDetailList = new HotelReservationDetailList() {
                    GuestId = int.Parse(authId),
                    HotelId = lastDetail.HotelId,
                    ReservationId = record.ReservationId,
                    StatusId = record.StatusId,
                    CurrencyId = lastDetail.CurrencyId,
                    HotelAccommodationActionId = lastDetail.HotelAccommodationActionId,
                    StartDate = lastDetail.StartDate,
                    EndDate = lastDetail.EndDate,
                    TotalPrice = lastDetail.TotalPrice,
                    Adult = lastDetail.Adult,
                    Children = lastDetail.Children,
                    Message = record.DetailMessage,
                    GuestSender = false,
                    Shown = false,
                    Timestamp = DateTimeOffset.Now.DateTime
                };
                var data = new hotelsContext().HotelReservationDetailLists.Add(reservationDetailList);
                int result = data.Context.SaveChanges();

                //update status in reservationList
                HotelReservationList reservationList;
                reservationList = new hotelsContext().HotelReservationLists.Where(a => a.Id == record.ReservationId).FirstOrDefault();
                reservationList.StatusId = record.StatusId;
                var data1 = new hotelsContext().HotelReservationLists.Update(reservationList);
                result = data1.Context.SaveChanges();

                //update status in reservationRoomList
                List<HotelReservedRoomList> reservedRoomList;
                reservedRoomList = new hotelsContext().HotelReservedRoomLists.Where(a => a.ReservationId == record.ReservationId).ToList();
                reservedRoomList.ForEach(async room => {
                    room.StatusId = record.StatusId;
                    var data2 = new hotelsContext().HotelReservedRoomLists.Update(room);
                    result = await data2.Context.SaveChangesAsync();
                });

                //send modified reservation email


                if (result > 0) {
                    return Ok(JsonSerializer.Serialize(
                    new DBResultMessage() {
                        Status = DBResult.success.ToString(),
                        ErrorMessage = string.Empty
                    }));
                }

            } catch { }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(),
                ErrorMessage = DBOperations.DBTranslate("BookingIsNotValid", record.Language)
            });
        }
    }
}