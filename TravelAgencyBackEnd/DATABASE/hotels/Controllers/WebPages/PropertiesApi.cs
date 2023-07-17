namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("WebApi/Properties")]
    public class PropertiesApi : ControllerBase {
        private readonly hotelsContext _dbContext = new();

        [HttpGet("/WebApi/Properties/{language}")]
        public async Task<string> GetProperties(string language = null) {
            List<PropertyOrServiceTypeList> result,resultNoGroup;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().PropertyOrServiceTypeLists
                    .Include(a => a.PropertyGroup).Where(a=> a.PropertyGroup != null)
                    .Include(a => a.PropertyOrServiceUnitType)
                    .OrderBy(a => a.PropertyGroup.Sequence)
                    .ToList();
            }

            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                resultNoGroup = new hotelsContext().PropertyOrServiceTypeLists
                    .Include(a => a.PropertyGroup).Where(a => a.PropertyGroup == null)
                    .Include(a => a.PropertyOrServiceUnitType)
                    .ToList();
            }
            result.AddRange(resultNoGroup);

            result.ForEach(item => {

                item.SystemName = DBOperations.DBTranslate(item.SystemName, language);
                item.PropertyOrServiceUnitType.SystemName = DBOperations.DBTranslate(item.PropertyOrServiceUnitType.SystemName, language);
                if (item.PropertyGroup != null) { 
                    item.PropertyGroup.SystemName = DBOperations.DBTranslate(item.PropertyGroup.SystemName, language);
                    item.PropertyGroup.PropertyOrServiceTypeLists = null;
                    item.PropertyOrServiceUnitType.PropertyOrServiceTypeLists = null;
                }
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

        [HttpGet("/WebApi/StateArea/{searchArea}/{language}")]
        public async Task<string> GetRoomTypes(string searchArea, string language = null) {
            List<HotelRoomTypeList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().HotelRoomTypeLists.ToList();
            }

            result.ForEach(item => { item.SystemName = DBOperations.DBTranslate(item.SystemName, language); });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}