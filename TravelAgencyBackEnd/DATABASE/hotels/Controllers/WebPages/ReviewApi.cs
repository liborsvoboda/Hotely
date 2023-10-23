using System;

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebApi")]
    public class ReviewApi : ControllerBase {

        [HttpPost("/WebApi/Guest/Review/AddReview")]
        [Consumes("application/json")]
        public IActionResult AddReview([FromBody] AddReview record) {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;

                HotelReservationReviewList hotelReservationReviewList = new();
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    hotelReservationReviewList = new hotelsContext().HotelReservationReviewLists.Where(a => a.ReservationId == record.ReservationId).FirstOrDefault();
                }

                if (hotelReservationReviewList == null) {
                    hotelReservationReviewList = new HotelReservationReviewList() {
                        HotelId = record.HotelId,ReservationId = record.ReservationId,
                        Description = record.Message,Rating = record.Rating,
                        GuestId = int.Parse(authId),
                        Answer = null, AdvertiserShown = null, Approved = null
                    
                    };
                    var data = new hotelsContext().HotelReservationReviewLists.Add(hotelReservationReviewList);
                    int result = data.Context.SaveChanges();

                    return Ok(JsonSerializer.Serialize(
                    new DBResultMessage() {
                        Status = DBResult.success.ToString(),
                        ErrorMessage = string.Empty
                    }));
                }
            } catch { }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(),
                ErrorMessage = DBOperations.DBTranslate("AddReviewtIsNotValid", record.Language)
            });
        }

    }
}