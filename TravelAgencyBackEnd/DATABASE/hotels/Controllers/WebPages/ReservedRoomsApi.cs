using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("WebApi/ReservedRoomList")]
    public class ReservedRoomsApi : ControllerBase {
        private readonly hotelsContext _dbContext = new();


        [HttpGet("/WebApi/ReservedRoomList/{hotelId}/{startDate}/{endDate}/{language}")]
        public async Task<string> GetReservedRoomsApi(int hotelId, DateTime startDate, DateTime endDate, string language = "cz") {
            List<HotelReservedRoomList> result;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().HotelReservedRoomLists
                    .Where(a => 
                    (a.StartDate <= startDate && a.EndDate > startDate) 
                    || (a.StartDate >= startDate && a.EndDate <= endDate)
                    || (a.StartDate < endDate && a.EndDate >= endDate)
                    )
                    .ToList();
            }

            return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}