namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("WebApi/Image")]
    public class WebImageApi : ControllerBase {
        private readonly hotelsContext _dbContext = new();

        [HttpGet("/WebApi/Image/{id}")]
        public async Task<IActionResult> GetSearchImageById(int? id = null) {
            int recId; HotelImagesList data = null;
            if (!string.IsNullOrWhiteSpace(id.ToString()) && int.TryParse(id.ToString(), out recId))
            {
                data = _dbContext.HotelImagesLists.Where(a => a.Id == recId).FirstOrDefault();
            }

            if (data != null) { return File(data.Attachment, MimeTypes.GetMimeType(data.FileName), data.FileName); }
            else { return BadRequest(new { message = DBOperations.DBTranslate("BadRequest", "en") }); }
        }

        [HttpGet("/WebApi/Image/{hotelId}/{filename}")]
        public async Task<IActionResult> GetSearchImageByKeys(int? hotelId = null, string fileName = null) {
            int recId; HotelImagesList data = null;
            if (!string.IsNullOrWhiteSpace(hotelId.ToString()) && !string.IsNullOrWhiteSpace(fileName) && int.TryParse(hotelId.ToString(), out recId))
            {
                data = _dbContext.HotelImagesLists.Where(a => a.HotelId == recId && a.FileName.ToLower() == fileName.ToLower()).FirstOrDefault();
            }

            if (data != null) { return File(data.Attachment, MimeTypes.GetMimeType(data.FileName), data.FileName); }
            else { return BadRequest(new { message = DBOperations.DBTranslate("BadRequest", "en") }); }
        }

        [HttpGet("/WebApi/RoomImage/{hotelId}/{roomId}")]
        public async Task<IActionResult> GetRoomImageByKeys(int? hotelId = null, int? roomId = null) {
            int hId; int rId; HotelRoomList data = null;
            if (int.TryParse(hotelId.ToString(), out hId) && int.TryParse(roomId.ToString(), out rId))
            {
                data = _dbContext.HotelRoomLists.Where(a => a.HotelId == hId && a.Id == rId).FirstOrDefault();
            }

            if (data != null) { return File(data.Image, MimeTypes.GetMimeType(data.ImageName), data.ImageName); }
            else { return BadRequest(new { message = DBOperations.DBTranslate("BadRequest", "en") }); }
        }
    }
}