using System;
using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebApi")]
    public class ReviewApi : ControllerBase {


        [HttpGet("/WebApi/Advertiser/GetReviewList")]
        public async Task<string> GetReviewList() {

            try {
                //string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;

                List<HotelReservationReviewList> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().HotelReservationReviewLists
                    .Where(a => a.Hotel.UserId == int.Parse(userId))
                    .ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) });
            }

        }


        [HttpGet("/WebApi/Advertiser/SetReviewShown/{id}")]
        public async Task<string> SetReviewShown(int id) {

            List<HotelReservationReviewList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().HotelReservationReviewLists.Where(a => !a.AdvertiserShown && a.HotelId == id).ToList();
            }

            int result = 0;
            data.ForEach(async review => {
                review.AdvertiserShown = true;
                var data = new hotelsContext().HotelReservationReviewLists.Update(review);
                result = await data.Context.SaveChangesAsync();
            });

            if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
        }


        [HttpPost("/WebApi/Guest/Review/AddReview")]
        [Consumes("application/json")]
        public IActionResult AddReview([FromBody] AddGuestReview record) {
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
                        Answer = null, AdvertiserShown = false, Approved = null
                    
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


        [HttpPost("/WebApi/Advertiser/SetAdvertiserReviewAnswer")]
        [Consumes("application/json")]
        public IActionResult SetAdvertiserReviewAnswer([FromBody] AddReviewAnswer record) {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;

                HotelReservationReviewList hotelReservationReviewList = new();
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    hotelReservationReviewList = new hotelsContext().HotelReservationReviewLists
                        .Where(a => a.Hotel.UserId == int.Parse(userId) && a.Id == record.ReviewId).FirstOrDefault();
                }

                if (hotelReservationReviewList != null) {
                    hotelReservationReviewList.Answer = record.Answer;
                    hotelReservationReviewList.Approved = null;
                    var data = new hotelsContext().HotelReservationReviewLists.Update(hotelReservationReviewList);
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