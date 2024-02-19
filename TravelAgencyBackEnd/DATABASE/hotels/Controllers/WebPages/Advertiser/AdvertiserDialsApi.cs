using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebApi/Advertiser")]
    public class AdvertiserDialsApi : ControllerBase {

        [HttpGet("/WebApi/Advertiser/GetCountryList/{language}")]
        public async Task<string> GetCountryList(string language = null) {

            List<CountryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().CountryLists.ToList();
            }

            data.ForEach(country => {
                country.SystemName = DbOperations.DBTranslate(country.SystemName, language);
            });

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/WebApi/Advertiser/GetCityList/{countryId}")]
        public async Task<string> GetCityList(int countryId) {
            List<CityList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().CityLists.Where(a => a.CountryId == countryId).ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/WebApi/Advertiser/GetCurrencyList")]
        public async Task<string> GetCurrencyList() {
            List<CurrencyList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().CurrencyLists.ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/WebApi/Advertiser/GetRoomTypeList/{language}")]
        public async Task<string> GetRoomTypeList(string language = null) {
            List<HotelRoomTypeList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().HotelRoomTypeLists.ToList();
            }

            data.ForEach(roomType => { roomType.SystemName = DbOperations.DBTranslate(roomType.SystemName, language); });

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/WebApi/Advertiser/GetStatusList/{language}")]
        public async Task<string> GetStatusList(string language = null) {
            List<HotelReservationStatusList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().HotelReservationStatusLists.OrderBy(a=>a.Sequence).ToList();
            }

            List<Translation> result = new List<Translation>();
            data.ForEach(status => {
                result.Add(new Translation() {
                    Id = status.Id,
                    SystemName = status.SystemName,
                    TranslationName = DbOperations.DBTranslate(status.SystemName, language)
                });
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/WebApi/Advertiser/GetRoomBookingList/{roomId}/{language}")]
        public async Task<string> GetRoomBookingList(int roomId, string language = "cz") {

            string userId = User.FindFirst(ClaimTypes.GroupSid.ToString()).Value;

            List<HotelReservedRoomList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().HotelReservedRoomLists
                    .Include(a => a.Reservation)
                    .Include(a=>a.Status)
                    .Where(a => a.HotelRoomId == roomId && a.Count > 0 && a.Hotel.UserId == int.Parse(userId))
                    .OrderBy(a=> a.HotelRoomId)
                    .OrderByDescending(a => a.Timestamp)
                    .ToList();
            }

            result.ForEach(reservation => {
                reservation.Status.SystemName = DbOperations.DBTranslate(reservation.Status.SystemName, language);
            });


            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

    }
}

