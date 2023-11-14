namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("WebApi/Properties")]
    public class PropertiesApi : ControllerBase {
        private readonly hotelsContext _dbContext = new();

        [HttpGet("/WebApi/Properties/{language}")]
        public async Task<string> GetProperties(string language = null) {
            List<PropertyOrServiceTypeList> result, resultNoGroup;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = _dbContext.PropertyOrServiceTypeLists
                    .Include(a => a.PropertyGroup).Where(a => a.PropertyGroup != null)
                    .Include(a => a.PropertyOrServiceUnitType)
                    .OrderBy(a => a.Sequence).ToList()
                    .OrderBy(a => a.PropertyGroup.Sequence).ToList();
            }

            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                resultNoGroup = _dbContext.PropertyOrServiceTypeLists
                    .Include(a => a.PropertyGroup).Where(a => a.PropertyGroup == null)
                    .Include(a => a.PropertyOrServiceUnitType)
                    .OrderBy(a => a.Sequence)
                    .ToList();
            }
            result.AddRange(resultNoGroup);

            result.ForEach(item => {

                item.SystemName = ServerCoreDbOperations.DBTranslate(item.SystemName, language);
                item.PropertyOrServiceUnitType.SystemName = ServerCoreDbOperations.DBTranslate(item.PropertyOrServiceUnitType.SystemName, language);
                if (item.PropertyGroup != null) {
                    item.PropertyGroup.SystemName = ServerCoreDbOperations.DBTranslate(item.PropertyGroup.SystemName, language);
                    item.PropertyGroup.PropertyOrServiceTypeLists = null;
                    item.PropertyOrServiceUnitType.PropertyOrServiceTypeLists = null;
                }
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        [HttpGet("/WebApi/RoomTypes/{language}")]
        public async Task<string> GetRoomTypes(string language = null) {
            List<HotelRoomTypeList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = _dbContext.HotelRoomTypeLists.ToList();
            }

            result.ForEach(item => { item.SystemName = ServerCoreDbOperations.DBTranslate(item.SystemName, language); });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }


        [HttpGet("/WebApi/StateArea/{searchArea}/{language}")]
        public async Task<string> GetRoomTypes(string searchArea, string language = null) {
            List<HotelList> check;
            List<CountryAreaList> result = new();
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                check = _dbContext.HotelLists
                    .Include(a => a.Country)
                    .ToList();
            }

            check.ForEach(item => {
                if (ServerCoreDbOperations.DBTranslate(item.Country.SystemName, language).ToLower() == searchArea.ToLower()) {
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        result = _dbContext.CountryAreaLists
                                .Where(a => a.CountryId == item.CountryId).ToList();
                    }
                    result.ForEach(item => {
                        item.SystemName = ServerCoreDbOperations.DBTranslate(item.SystemName, language);
                    });
                }
            });

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        [HttpGet("/WebApi/Properties/GetPropertyGroupList/{language}")]
        public async Task<string> GetPropertyGroupList(string language = null) {
            List<PropertyGroupList> result, resultNoGroup;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = _dbContext.PropertyGroupLists
                    .OrderBy(a => a.Sequence).ToList()
                    ;
            }

            result.ForEach(propertyGroup => {
                propertyGroup.SystemName = ServerCoreDbOperations.DBTranslate(propertyGroup.SystemName, language);
            });

            //more filters
            //PropertyGroupList morefilter = new PropertyGroupList() { 
            //SystemName = DBOperations.DBTranslate("moreFilters", language),
            //};
            //result.Add(morefilter);

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }


    }
}