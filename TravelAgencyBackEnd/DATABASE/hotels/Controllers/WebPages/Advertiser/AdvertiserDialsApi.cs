namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebApi/Advertiser")]
    public class AdvertiserDialsApi : ControllerBase {

        [HttpGet("/WebApi/Advertiser/GetCountryList")]
        public async Task<string> GetCountryList() {

            List<CountryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().CountryLists.ToList();
            }

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

            data.ForEach(roomType => { roomType.SystemName = DBOperations.DBTranslate(roomType.SystemName, language); });

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

    }
}