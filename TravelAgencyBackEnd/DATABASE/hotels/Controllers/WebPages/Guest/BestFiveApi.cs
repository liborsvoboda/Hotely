using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("WebApi/Guest")]
    public class BestFiveApi : ControllerBase {


        [Authorize]
        [HttpGet("/WebApi/Guest/GetBestFiveList/{type}/{language}")]
        public async Task<string> GetBestFiveList(string type = "cz", string language = "cz") {

            string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
            List<int> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().GuestFavoriteLists
                    .Where(a => a.GuestId == int.Parse(authId)).OrderByDescending(a => a.TimeStamp).Select(a => a.HotelId).ToList();
            }

            List<HotelList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().HotelLists
                    .Include(a => a.HotelRoomLists.Where(a => a.Approved == true))
                    .Include(a => a.City)
                    .Include(a => a.Country)
                    .Include(a => a.DefaultCurrency)
                    .Include(a => a.HotelPropertyAndServiceLists)
                    .Include(a => a.HotelImagesLists)
                    .Where(a => data.Contains(a.Id)).ToList();
            }

            result.ForEach(hotel => {
                hotel.DescriptionCz = hotel.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "");
                hotel.DescriptionEn = hotel.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "");
                hotel.HotelRoomLists.ToList().ForEach(hotelRoom => {
                    hotelRoom.DescriptionCz = hotelRoom.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "");
                    hotelRoom.DescriptionEn = hotelRoom.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "");
                });
                hotel.HotelImagesLists.ToList().ForEach(attachment => { attachment.Attachment = null; });
            });

            //TODO changed to old structure
            WebPageRootSearchData rootData = new();
            result.ForEach(hotel => { rootData.HotelList.Add(new WebPageRootSearch() { Hotel = hotel, RoomList = null }); });

            return JsonSerializer.Serialize(rootData, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}