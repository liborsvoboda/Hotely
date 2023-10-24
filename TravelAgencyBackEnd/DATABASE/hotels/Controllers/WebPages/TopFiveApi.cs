using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("WebApi/Guest")]
    public class TopFiveApi : ControllerBase {


        [HttpGet("/WebApi/Guest/GetTopFiveList/{type}/{language}")]
        public async Task<string> GetTopFiveList(string type, string language = "cz") {

            List<int> data = new();

            switch (type) {
                case "newest":
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        data = new hotelsContext().HotelLists.Where(a => a.Approved && a.Advertised && !a.Deactivated)
                            .OrderByDescending(a => a.Timestamp).Select(a => a.Id).Take(5).ToList();
                    }
                    break;
                case "cheapest":
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        data = new hotelsContext().HotelRoomLists
                            .Where(a => a.Hotel.Approved && a.Hotel.Advertised && !a.Hotel.Deactivated)
                            .OrderBy(a => a.Price).Select(a => a.HotelId).Distinct().Take(5).ToList();
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
                    .Include(a => a.HotelRoomLists)
                    .Include(a => a.City)
                    .Include(a => a.Country)
                    .Include(a => a.DefaultCurrency)
                    .Where(a => data.Contains(a.Id)).ToList();
            }

            result.ForEach(hotel => {

                // load props to each hotel by include is extremelly slow
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    List<HotelImagesList> images = new hotelsContext().HotelImagesLists.Where(a => a.HotelId == hotel.Id).ToList();
                    hotel.HotelImagesLists = images;

                    List<HotelPropertyAndServiceList> props = new hotelsContext().HotelPropertyAndServiceLists.Where(a => a.HotelId == hotel.Id && a.IsAvailable).ToList();
                    hotel.HotelPropertyAndServiceLists = props;

                    List<HotelReservationReviewList> reviews = new hotelsContext().HotelReservationReviewLists.Where(a => a.HotelId == hotel.Id && a.Approved == true)
                    .OrderByDescending(a => a.Timestamp).ToList();
                    hotel.HotelReservationReviewLists = reviews;
                }


                //clean datasets
                hotel.DescriptionCz = hotel.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
                hotel.DescriptionEn = hotel.DescriptionEn != null ? hotel.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>") : "";
                hotel.HotelRoomLists.ToList().ForEach(hotelRoom => {
                    hotelRoom.DescriptionCz = hotelRoom.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
                    hotelRoom.DescriptionEn = hotelRoom.DescriptionEn != null ? hotelRoom.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>") : "";
                });
                hotel.HotelImagesLists.ToList().ForEach(attachment => {
                    attachment.Hotel = null;
                    attachment.Attachment = null;
                });

                hotel.City.HotelLists = null;
                hotel.City.Country = null;
                hotel.Country.CityLists = null;
                hotel.Country.HotelLists = null;
                hotel.DefaultCurrency.HotelLists = null;
                hotel.HotelRoomLists.ToList().ForEach(room => {
                    room.Image = null;
                    room.Hotel = null;
                });
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