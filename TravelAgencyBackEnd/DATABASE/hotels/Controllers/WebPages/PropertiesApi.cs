namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("WebApi/Properties")]
    public class PropertiesApi : ControllerBase {
        private readonly hotelsContext _dbContext = new();

        [HttpGet("/WebApi/Properties/{language}")]
        public async Task<string> GetProperties(string language = null) {
            List<PropertyOrServiceTypeList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                result = new hotelsContext().PropertyOrServiceTypeLists
                    .Include(a => a.PropertyOrServiceUnitType)
                    .ToList();
            }

            result.ForEach(item =>
            {
                item.SystemName = DBOperations.DBTranslate(item.SystemName, language);
                item.PropertyOrServiceUnitType.SystemName = DBOperations.DBTranslate(item.PropertyOrServiceUnitType.SystemName, language);
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        [HttpGet("/WebApi/RoomTypes/{language}")]
        public async Task<string> GetRoomTypes(string language = null) {
            List<HotelRoomTypeList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                result = new hotelsContext().HotelRoomTypeLists.ToList();
            }

            result.ForEach(item => { item.SystemName = DBOperations.DBTranslate(item.SystemName, language); });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}