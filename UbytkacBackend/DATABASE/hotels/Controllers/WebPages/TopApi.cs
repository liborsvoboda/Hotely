using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("WebApi/Top")]
    public class TopApi : ControllerBase {

        [HttpGet("/WebApi/Top/GetMainTopList/{language}")]
        public async Task<string> GetTopList(string language = "cz") {
            List<int> data = GetTopIdList(language);

            List<HotelList> result;
            WebPageRootSearchData rootData = new();
            try { 
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    result = new hotelsContext().HotelLists
                        .Include(a => a.HotelRoomLists)
                        .Include(a => a.City)
                        .Include(a => a.Country)
                        .Include(a => a.DefaultCurrency)
                        .Where(a => data.Contains(a.Id) && !a.Deactivated).ToList();
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
                        room.DescriptionCz = room.DescriptionCz.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>");
                        room.DescriptionEn = room.DescriptionEn != null ? room.DescriptionEn.Replace("<HTML><BODY>", "").Replace("</BODY></HTML>", "").Replace("<LI><P>", "<LI><SPAN>").Replace("</P></LI>", "</SPAN></LI>") : "";
                        room.Image = null;
                        room.Hotel = null;
                    });
                });

                //TODO changed to old structure
               
                result.ForEach(hotel => { rootData.HotelList.Add(new WebPageRootSearch() { Hotel = hotel, RoomList = null }); });

            } 
            catch (Exception ex) { }

            return JsonSerializer.Serialize(rootData, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }


        private List<int> GetTopIdList(string language = "cz") {
            List<int> searchedIdList = new();
            List<int> otherData;

            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                searchedIdList = new hotelsContext().HotelLists.Where(a => a.Top && !a.Deactivated)
                    .Where(a => a.Approved && a.Advertised && a.TotalCapacity > 0)
                    .OrderByDescending(a=> a.TopDate).Select(a => a.Id).ToList();
            }

            if (searchedIdList.Count < 20) {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    otherData = new hotelsContext().HotelLists.Where(a => !a.Top && !a.Deactivated)
                        .Where(a => a.Approved && a.Advertised && a.TotalCapacity > 0)
                        .OrderBy(a => a.LastTopShown).Select(a => a.Id).Take(20 - searchedIdList.Count).ToList();
                }
                searchedIdList.AddRange(otherData);
            }


            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter { ParameterName = "@IdList", Value = string.Join(";", searchedIdList) } };
            new hotelsContext().Database.ExecuteSqlRaw("exec SpTriggerSetTopShown @IdList", parameters.ToArray());

            return searchedIdList;
        }
    }
}