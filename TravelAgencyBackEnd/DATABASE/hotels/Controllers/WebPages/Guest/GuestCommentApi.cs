using System;

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebApi/Guest")]
    public class GuestCommentApi : ControllerBase {

        [HttpPost("/WebApi/Guest/SetGuestComment")]
        [Consumes("application/json")]
        public IActionResult SetGuestComment([FromBody] WebGuestComment record) {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                GuestAdvertiserNoteList guestAdvertiserNoteList = new() { Title = record.Title,Note = record.Note, HotelId = record.HotelId, GuestId = int.Parse(authId) };
                var data = new hotelsContext().GuestAdvertiserNoteLists.Add(guestAdvertiserNoteList);
                int result = data.Context.SaveChanges();

                return Ok(JsonSerializer.Serialize(
                new DBResultMessage() {
                    Status = DBResult.success.ToString(),
                    ErrorMessage = string.Empty
                }));
            } catch { }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(),
                ErrorMessage = DBOperations.DBTranslate("AdvertiseCommentIsNotValid", record.Language)
            });
        }

        [HttpGet("/WebApi/Guest/SetCommentStatus/{commentId}/{language}")]
        [Consumes("application/json")]
        public IActionResult SetCommentStatusByCommentId(int commentId, string language = "cz") {
            GuestAdvertiserNoteList guestAdvertiserNoteList;
            try {
                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    guestAdvertiserNoteList = new hotelsContext().GuestAdvertiserNoteLists.Where(a => a.Id == commentId && a.GuestId == int.Parse(authId)).FirstOrDefault();
                }

                guestAdvertiserNoteList.Solved = !guestAdvertiserNoteList.Solved;
                var data = new hotelsContext().GuestAdvertiserNoteLists.Update(guestAdvertiserNoteList);
                int result = data.Context.SaveChanges();

                return Ok(JsonSerializer.Serialize(
                new DBResultMessage() {
                    Status = DBResult.success.ToString(),
                    ErrorMessage = string.Empty
                }));

            } catch { }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(),
                ErrorMessage = DBOperations.DBTranslate("AdvertiseCommentIsNotValid", language)
            });
        }

        [HttpGet("/WebApi/Guest/DeleteComment/{commentId}/{language}")]
        [Consumes("application/json")]
        public IActionResult DeleteCommentByCommentId(int commentId, string language = "cz") {
            GuestAdvertiserNoteList guestAdvertiserNoteList;
            try {
                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    guestAdvertiserNoteList = new hotelsContext().GuestAdvertiserNoteLists.Where(a => a.Id == commentId && a.GuestId == int.Parse(authId)).FirstOrDefault();
                }

                var data = new hotelsContext().GuestAdvertiserNoteLists.Remove(guestAdvertiserNoteList);
                int result = data.Context.SaveChanges();

                return Ok(JsonSerializer.Serialize(
                new DBResultMessage() {
                    Status = DBResult.success.ToString(),
                    ErrorMessage = string.Empty
                }));

            } catch { }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(),
                ErrorMessage = DBOperations.DBTranslate("AdvertiseCommentIsNotValid", language)
            });
        }
    }
}