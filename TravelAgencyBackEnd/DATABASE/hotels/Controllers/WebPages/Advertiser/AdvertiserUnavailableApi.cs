using System;

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebApi/Advertiser")]
    public class AdvertiserUnavailableApi : ControllerBase {
        

        [HttpGet("/WebApi/Advertiser/GetUnavailableRooms/{hotelId}")]
        public async Task<string> GetUnavailableRooms(int hotelId) {

            string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;

            List<HotelReservedRoomList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().HotelReservedRoomLists.Where(a=> a.HotelId == hotelId && a.Hotel.UserId == int.Parse(userId) && a.StatusId == 5).ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }


        [HttpPost("/WebApi/Advertiser/SetUnavailableRooms")]
        [Consumes("application/json")]
        public IActionResult SetUnavailableRooms([FromBody] UnavailableRooms record) {
            try {

                string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;

                //Get room Types
                List<HotelRoomList> roomTypes;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    roomTypes = new hotelsContext().HotelRoomLists.Where(a => a.HotelId == record.HotelId && a.UserId == int.Parse(userId)).ToList();
                }

                if (roomTypes != null) {
                    //Save All Unavailable Rooms
                    record.RoomsId.ForEach(roomId => {
                        HotelReservedRoomList unavailableRoom = new() {
                            HotelId = record.HotelId,
                            HotelRoomId = roomId,
                            ReservationId = null,
                            RoomTypeId = roomTypes.First(a => a.Id == roomId).RoomTypeId,
                            StatusId = 5,
                            Name = roomTypes.First(a => a.Id == roomId).Name,
                            Count = roomTypes.First(a => a.Id == roomId).RoomsCount,
                            StartDate = record.StartDate,
                            EndDate = record.EndDate,
                            ExtraBed = false
                        };
                        var data = new hotelsContext().HotelReservedRoomLists.Add(unavailableRoom);
                        int result = data.Context.SaveChanges();
                    });

                    return Ok(JsonSerializer.Serialize(
                    new DBResultMessage() {
                        Status = DBResult.success.ToString(),
                        ErrorMessage = string.Empty
                    }));
                }
            } catch { }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(),
                ErrorMessage = DbOperations.DBTranslate("AdvertiseUnavailableIsNotValid", record.Language)
            });
        }



        [HttpGet("/WebApi/Advertiser/DeleteUnavailableRoom/{recId}/{language}")]
        [Consumes("application/json")]
        public IActionResult DeleteCommentByCommentId(int recId, string language = "cz") {

            HotelReservedRoomList unavailableRoom;
            try {
                string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;

                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    unavailableRoom = new hotelsContext().HotelReservedRoomLists.Where(a => a.Id == recId && a.Hotel.UserId == int.Parse(userId)).FirstOrDefault();
                }

                if (unavailableRoom != null) {
                    var data = new hotelsContext().HotelReservedRoomLists.Remove(unavailableRoom);
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
                ErrorMessage = DbOperations.DBTranslate("AdvertiseUnavailableIsNotValid", language)
            });
        }

    }
}