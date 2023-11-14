using Microsoft.AspNetCore.Mvc.Infrastructure;
using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

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
                        .Include(a => a.HotelReservationDetailLists).ThenInclude(a=> a.Status)
                        .Include(a => a.HotelReservedRoomLists.Where(a => a.Count > 0))
                        .Include(a => a.Status)
                        .Include(a => a.Hotel).ThenInclude(a => a.HotelImagesLists)
                        .Include(a => a.Currency)
                        .Include(a=>a.HotelReservationReviewList)
                        .OrderByDescending(a => a.Timestamp).ToList();
                }

                data.ForEach(reservation => {
                    reservation.Currency.HotelLists = null;
                    reservation.Currency.HotelReservationLists = null;
                    reservation.Currency.HotelReservationDetailLists = null;
                    reservation.Status.SystemName = ServerCoreDbOperations.DBTranslate(reservation.Status.SystemName, language);
                    reservation.Hotel.HotelImagesLists.ToList().ForEach(image => {
                        image.Hotel = null;
                        image.Attachment = null;
                    });

                    if (reservation.HotelReservationDetailLists != null) {
                        reservation.HotelReservationDetailLists.ToList().ForEach(reservationDetail => {
                            reservationDetail.Hotel = null;
                            reservationDetail.Reservation = null;
                            if (reservationDetail.Status != null) {
                                reservationDetail.Status.SystemName = ServerCoreDbOperations.DBTranslate(reservationDetail.Status.SystemName, language);
                                reservationDetail.Status.HotelReservationLists = null;
                                reservationDetail.Status.HotelReservedRoomLists = null;
                                reservationDetail.Status.HotelReservationDetailLists = null;
                            }
                        });
                    }

                    //oposite sort of part
                    reservation.HotelReservationDetailLists = reservation.HotelReservationDetailLists != null ? reservation.HotelReservationDetailLists.OrderByDescending(a=>a.Timestamp).ToList() : null;

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

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
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
                ErrorMessage = ServerCoreDbOperations.DBTranslate("BookingIsNotValid", record.Language)
            });
        }


        [Authorize]
        [HttpPost("/WebApi/Guest/Booking/CancelRequestBooking")]
        [Consumes("application/json")]
        public IActionResult CancelRequestBooking([FromBody] BookingCancel record) {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                HotelReservationList reservationList;
                reservationList = new hotelsContext().HotelReservationLists.Where(a => a.GuestId == int.Parse(authId) && a.Id == record.ReservationId).FirstOrDefault();

                if (reservationList != null) {
                    HotelReservationDetailList reservationDetailList = new HotelReservationDetailList() {
                        GuestId = int.Parse(authId),
                        HotelId = reservationList.HotelId,
                        ReservationId = record.ReservationId,
                        StatusId = 4,
                        CurrencyId = reservationList.CurrencyId,
                        HotelAccommodationActionId = reservationList.HotelAccommodationActionId,
                        StartDate = reservationList.StartDate,
                        EndDate = reservationList.EndDate,
                        TotalPrice = reservationList.TotalPrice,
                        Adult = reservationList.Adult,
                        Children = reservationList.Children,
                        Message = record.Message,
                        GuestSender = true,
                        Shown = false,
                        Timestamp = DateTimeOffset.Now.DateTime
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
                ErrorMessage = ServerCoreDbOperations.DBTranslate("BookingIsNotValid", record.Language)
            });
        }


        [Authorize]
        [HttpPost("/WebApi/Guest/Booking/UpdateBooking")]
        [Consumes("application/json")]
        public IActionResult UpdateBooking([FromBody] UpdateBookingData record) {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;

                HotelReservationDetailList reservationDetailList = new HotelReservationDetailList() {
                    GuestId = int.Parse(authId),
                    HotelId = record.Booking.HotelId, ReservationId = record.Booking.Id,
                    StatusId = record.Booking.StatusId, CurrencyId = record.Booking.CurrencyId,
                    HotelAccommodationActionId = record.Booking.HotelAccommodationActionId,
                    StartDate = record.Booking.StartDate, EndDate = record.Booking.EndDate,
                    TotalPrice = record.Booking.TotalPrice,
                    Adult = record.Booking.Adult, Children = record.Booking.Children,
                    Message = record.Booking.Message,
                    GuestSender = true,
                    Shown = false,
                    Timestamp = DateTimeOffset.Now.DateTime
                };
                var data = new hotelsContext().HotelReservationDetailLists.Add(reservationDetailList);
                int result = data.Context.SaveChanges();

                //update address in reservationList
                HotelReservationList reservationList;

                reservationList = new hotelsContext().HotelReservationLists.Where(a=> a.GuestId == int.Parse(authId) && a.Id == record.Booking.Id).FirstOrDefault();
                reservationList.FirstName = record.Booking.FirstName;
                reservationList.LastName = record.Booking.LastName;
                reservationList.Street = record.Booking.Street;
                reservationList.Zipcode = record.Booking.ZipCode;
                reservationList.City = record.Booking.City;
                reservationList.Country = record.Booking.Country;
                reservationList.Phone = record.Booking.Phone;
                reservationList.Email = record.Booking.Email;

                var data1 = new hotelsContext().HotelReservationLists.Update(reservationList);
                result = data1.Context.SaveChanges();

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
                ErrorMessage = ServerCoreDbOperations.DBTranslate("BookingIsNotValid", record.Language)
            });
        }
    }
}