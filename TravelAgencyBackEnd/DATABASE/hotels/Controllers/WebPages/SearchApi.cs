namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("WebApi/Search")]
    public class WebSearchApi : ControllerBase {
        private readonly hotelsContext _dbContext = new();

        [HttpGet("/WebApi/Search/GetSearchDialList/{language}")]
        public async Task<string> GetSearchDialList(string language = "cz") {
            List<string> data = GetTranslatedSearchList(language);

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }


        [HttpGet("/WebApi/Search/GetSearchAreaList/{language}")]
        public async Task<string> GetSearchAreaList(string language = "cz") {
            List<string> data = new();

            List<InterestAreaList> areaList = new List<InterestAreaList>();
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                areaList = new hotelsContext().InterestAreaLists.ToList();
            }
            areaList.ForEach(item => data.Add(DBOperations.DBTranslate(item.SystemName, language)));

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }


        [HttpGet("/WebApi/Search/GetSearchInput/{searched}/{language}")]
        public async Task<string> GetSearchInput(string searched, string language = "cz") {
            List<int> searchedIdList = GetSearchedIdList(searched, language);

            List<HotelList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                result = new hotelsContext().HotelLists
                    .Include(a => a.HotelRoomLists)
                    .Include(a => a.City)
                    .Include(a => a.Country)
                    .Include(a => a.DefaultCurrency)
                    .Where(a => searchedIdList.Contains(a.Id) && !a.Deactivated).ToList();
            }

            result.ForEach(hotel => {

                // load props to each hotel by include is extremelly slow
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    List<HotelImagesList> images = new hotelsContext().HotelImagesLists.Where(a => a.HotelId == hotel.Id).ToList();
                    hotel.HotelImagesLists = images;

                    List<HotelPropertyAndServiceList> props = new hotelsContext().HotelPropertyAndServiceLists.Where(a => a.HotelId == hotel.Id && a.IsAvailable).ToList();
                    hotel.HotelPropertyAndServiceLists = props;

                    List<HotelReservationReviewList> reviews = new hotelsContext().HotelReservationReviewLists.Where(a => a.HotelId == hotel.Id && a.Approved == true)
                    .OrderByDescending(a=> a.Timestamp).ToList();
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


            //Update advertisement shown count
            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter { ParameterName = "@IdList", Value = string.Join(";", searchedIdList) } };
            new hotelsContext().Database.ExecuteSqlRaw("exec SetShown @IdList", parameters.ToArray());


            //TODO changed to old structure
            WebPageRootSearchData rootData = new();
            result.ForEach(hotel => { rootData.HotelList.Add(new WebPageRootSearch() { Hotel = hotel, RoomList = null }); });

            return JsonSerializer.Serialize(rootData, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        /// <summary>
        /// Full Translate List for WebPage Search Input : Whisperer
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private List<string> GetTranslatedSearchList(string language = "cz") {
            List<string> data;
            List<string> cityData;
            List<string> countryData;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                countryData = _dbContext.CountryLists.
                    Join(_dbContext.HotelLists.Where(a => a.Advertised && a.Approved && !a.Deactivated),
                    joiner => joiner.Id, joined => joined.CountryId, (_joiner, _joined) => _joiner.SystemName).ToList();

                cityData = _dbContext.CityLists.Join(_dbContext.HotelLists.Where(a => a.Advertised && a.Approved == true && !a.Deactivated),
                    joiner => joiner.Id, joined => joined.CityId, (_joiner, _joined) => _joiner.City).ToList();

                data = _dbContext.HotelLists.Where(a => a.Approved == true && a.Advertised == true && !a.Deactivated).Select(a => a.Name).ToList();
            }
            countryData.ForEach(item => data.Add(DBOperations.DBTranslate(item, language)));
            cityData.ForEach(item => data.Add(DBOperations.DBTranslate(item, language)));
            data = data.Distinct().ToList();
            data.Sort();
            return data;
        }

        /// <summary>
        /// Full Translate List for WebPage Search Input
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private List<int> GetSearchedIdList(string searched, string language = "cz") {
            List<InterestAreaList> areaData;
            List<Tuple<int, string>> data;
            List<Tuple<int, string>> cityData;
            List<Tuple<int, string>> countryData;
            List<int> searchedIdList = new();

            if (searched != "null") {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    areaData = _dbContext.InterestAreaLists.ToList();
                }

                //Insert All hotel Id from Selected Area by AreaCityId
                areaData.ForEach(area => {
                    List<int> cityIdList;
                    if (DBOperations.DBTranslate(area.SystemName, language).ToLower() == searched.ToLower()) {
                        using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                            cityIdList = _dbContext.InterestAreaCityLists.Where(a => a.Iacid == area.Id).Select(a => a.CityId).ToList();
                            searchedIdList = _dbContext.HotelLists.Where(a => cityIdList.Contains(a.CityId) && !a.Deactivated).Select(a => a.Id).ToList();
                        }
                    }
                });
            }

            //Area not found fill by Other  
            if (!searchedIdList.Any()) {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {

                    countryData = _dbContext.CountryLists.
                        Join(_dbContext.HotelLists.Where(a => a.Advertised && a.Approved == true && !a.Deactivated),
                        joiner => joiner.Id, joined => joined.CountryId, (_joiner, _joined) => new Tuple<int, string>(_joined.Id, _joiner.SystemName)).ToList();

                    cityData = _dbContext.CityLists.Join(_dbContext.HotelLists.Where(a => a.Advertised && a.Approved == true && !a.Deactivated),
                        joiner => joiner.Id, joined => joined.CityId, (_joiner, _joined) => new Tuple<int, string>(_joined.Id, _joiner.City)).ToList();

                    data = _dbContext.HotelLists.Where(a => a.Approved == true && a.Advertised == true && !a.Deactivated).Select(a => new Tuple<int, string>(a.Id, a.Name)).ToList();
                }

                countryData.ForEach(item => data.Add(new Tuple<int, string>(item.Item1, DBOperations.DBTranslate(item.Item2, language))));
                cityData.ForEach(item => data.Add(new Tuple<int, string>(item.Item1, DBOperations.DBTranslate(item.Item2, language))));
                data = data.Distinct().ToList();
                data.Sort();

                //Check Searched Value
                data.ForEach(item => {
                    if (searched != "null") {
                        if (item.Item2.ToLower().Contains(searched.ToLower())) searchedIdList.Add(item.Item1);
                    } else {
                        searchedIdList.Add(item.Item1);
                    }
                });
            }

            return searchedIdList;
        }
    }
}