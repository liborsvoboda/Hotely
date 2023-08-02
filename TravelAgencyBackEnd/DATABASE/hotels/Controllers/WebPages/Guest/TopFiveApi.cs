using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("WebApi/Guest")]
    public class TopFiveApi : ControllerBase {


        [Authorize]
        [HttpGet("/WebApi/Guest/GetTopFiveList/{type}/{language}")]
        public async Task<string> GetTopFiveList(string type, string language = "cz") {

            List<int> data = new();

            switch (type) {
                case "newest":
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        data = new hotelsContext().HotelLists.Where(a=>a.Approved && a.Advertised)
                            .OrderByDescending(a => a.Timestamp).Select(a => a.Id).Take(5).ToList();
                    }
                    break;
                case "cheapest":
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        data = new hotelsContext().HotelRoomLists
                            .Where(a=> a.Approved && a.Hotel.Approved && a.Hotel.Advertised)
                            .OrderBy(a => a.Price).Select(a=>a.HotelId).Distinct().Take(5).ToList();
                    }
                    break;
                case "favoritest":
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        data = new hotelsContext().GetTopFiveFavoriteLists.Select(a=> a.Id).ToList();
                    }
                    break;
            }

            List<HotelList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().HotelLists
                    .Include(a => a.HotelRoomLists.Where(a => a.Approved == true))
                    .Include(a => a.City)
                    .Include(a => a.Country)
                    .Include(a => a.DefaultCurrency)
                    .Include(a => a.HotelPropertyAndServiceLists.Where(a => a.IsAvailable))
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