using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("WebApi/Guest")]
    public class FavoriteApi : ControllerBase {

        [Authorize]
        [HttpGet("/WebApi/Guest/GetFavoriteList")]
        [Consumes("application/json")]
        public async Task<string> GetFavoriteList() {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                List<GuestFavoriteList> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().GuestFavoriteLists.Where(a => a.GuestId == int.Parse(authId)).OrderByDescending(a => a.TimeStamp).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [Authorize]
        [HttpGet("/WebApi/Guest/SetFavorite/{hotelId}")]
        [Consumes("application/json")]
        public async Task<string> SetFavorite(string hotelId) {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                GuestFavoriteList data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().GuestFavoriteLists
                        .Where(a => a.GuestId == int.Parse(authId) && a.HotelId == int.Parse(hotelId)).FirstOrDefault();
                }

                if (data == null) {
                    data = new GuestFavoriteList() {
                        GuestId = int.Parse(authId),
                        HotelId = int.Parse(hotelId),
                        TimeStamp = DateTimeOffset.Now.DateTime
                    };
                    var resultData = new hotelsContext().GuestFavoriteLists.Add(data);
                    int result = await resultData.Context.SaveChangesAsync();
                } else {
                    var resultData = new hotelsContext().GuestFavoriteLists.Remove(data);
                    int result = await resultData.Context.SaveChangesAsync();
                }

                List<GuestFavoriteList> finalData;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    finalData = new hotelsContext().GuestFavoriteLists
                        .Where(a => a.GuestId == int.Parse(authId)).OrderByDescending(a=> a.TimeStamp).ToList();
                }

                return JsonSerializer.Serialize(finalData, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }


        [Authorize]
        [HttpGet("/WebApi/Guest/GetFavoriteList/{language}")]
        public async Task<string> GetFavoriteList(string language = "cz") {

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