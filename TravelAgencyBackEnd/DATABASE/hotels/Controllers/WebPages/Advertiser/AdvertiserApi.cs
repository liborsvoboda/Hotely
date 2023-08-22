using static UbytkacBackend.CoreClasses.ApiClassesExtension;

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebApi/Advertiser")]
    public class AdvertiserApi : ControllerBase {

        [HttpGet("/WebApi/Advertiser/GetAdvertisementList/{language}")]
        public async Task<string> GetAdvertisementList(string language = null) {

            string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;
            List<HotelList> result = new List<HotelList>();
            try {

                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    result = new hotelsContext().HotelLists
                        .Include(a => a.HotelRoomLists)
                        .Include(a => a.City)
                        .Include(a => a.Country)
                        .Include(a => a.DefaultCurrency)
                        .Where(a => a.UserId == int.Parse(userId))
                        .AsNoTracking()
                        .IgnoreAutoIncludes()
                        .ToList();
                }


                result.ForEach(hotel => {

                    // load props to each hotel by include is extremelly slow
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        List<HotelImagesList> images = new hotelsContext().HotelImagesLists.Where(a => a.HotelId == hotel.Id).ToList();
                        hotel.HotelImagesLists = images;

                        List<HotelPropertyAndServiceList> props = new hotelsContext().HotelPropertyAndServiceLists.Where(a => a.HotelId == hotel.Id && a.IsAvailable).ToList();
                        hotel.HotelPropertyAndServiceLists = props;
                    }

                    //clean datasets
                    hotel.DescriptionCz = hotel.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "");
                    hotel.DescriptionEn = hotel.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "");

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

                //result.ForEach(item => { item.SystemName = DBOperations.DBTranslate(item.SystemName, language); });
            } 
            catch (Exception ex) { }
            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }



    }
}