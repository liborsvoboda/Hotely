using Microsoft.AspNetCore.Mvc.Infrastructure;
using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("WebApi/Guest")]
    public class BookingApi : ControllerBase {

        [Authorize]
        [HttpGet("/WebApi/Guest/Booking/GetBookingList/{language}")]
        [Consumes("application/json")]
        public async Task<string> GetBookingList(string language = "cz") {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                List<HotelReservationList> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().HotelReservationLists.Where(a => a.GuestId == int.Parse(authId))
                        .Include(a => a.HotelReservationDetailLists)
                        .Include(a => a.HotelReservedRoomLists)
                        .Include(a => a.Status)
                        .Include(a => a.Hotel).ThenInclude(a => a.HotelImagesLists)
                        .Include(a => a.Currency)
                        .OrderByDescending(a => a.Timestamp).ToList();
                }

                data.ForEach(reservation => {
                    reservation.Currency.HotelLists = null;
                    reservation.Currency.HotelReservationLists = null;
                    reservation.Currency.HotelReservationDetailLists = null;
                    reservation.Status.SystemName = DBOperations.DBTranslate(reservation.Status.SystemName, language);
                    reservation.Hotel.HotelImagesLists.ToList().ForEach(image => {
                        image.Hotel = null;
                        image.Attachment = null; 
                    });

                    reservation.HotelReservationDetailLists.ToList().ForEach(reservationDetail => {
                        reservationDetail.Hotel = null;
                        reservationDetail.Reservation = null;
                        reservationDetail.Status.HotelReservationLists = null;
                        reservationDetail.Status.HotelReservedRoomLists = null;
                        reservationDetail.Status.HotelReservationDetailLists = null;
                    });
                    reservation.HotelReservedRoomLists.ToList().ForEach(room => {
                        room.Hotel = null;
                        room.Reservation = null;
                        room.Status = null;
                    });
                    reservation.Hotel.HotelReservationLists = null;
                    reservation.Hotel.HotelReservedRoomLists = null;
                    reservation.Hotel.HotelReservationDetailLists = null;
                });

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [Authorize]
        [HttpPost("/WebApi/Guest/Booking/CancelBooking")]
        [Consumes("application/json")]
        public IActionResult CancelBooking([FromBody] BookingCancel record) {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                HotelReservationList reservationList;
                reservationList = new hotelsContext().HotelReservationLists.Where(a => a.GuestId == int.Parse(authId) && a.Id == record.ReservationId).FirstOrDefault();

                if (reservationList != null) {
                    HotelReservationDetailList reservationDetailList = new HotelReservationDetailList() {
                        GuestId = int.Parse(authId), HotelId = reservationList.HotelId, ReservationId = record.ReservationId, StatusId = 3,
                        CurrencyId = reservationList.CurrencyId, HotelAccommodationActionId = reservationList.HotelAccommodationActionId,
                        StartDate = reservationList.StartDate, EndDate = reservationList.EndDate,
                        TotalPrice = reservationList.TotalPrice, Adult = reservationList.Adult, Children = reservationList.Children,
                        Message = record.Message, GuestSender = true, Shown = false, Timestamp = DateTimeOffset.Now.DateTime
                    };
                    var data = new hotelsContext().HotelReservationDetailLists.Add(reservationDetailList);
                    int result = data.Context.SaveChanges();

                    if (result > 0) {
                        return Ok(JsonSerializer.Serialize(
                        new DBResultMessage() {
                            Status = DBResult.success.ToString(),
                            ErrorMessage = string.Empty
                        }));
                    }
                }
            } catch { }
            return BadRequest(new DBResultMessage() {
            Status = DBResult.error.ToString(),
                ErrorMessage = DBOperations.DBTranslate("BookingIsNotValid", record.Language)
            });


        }
    }
}