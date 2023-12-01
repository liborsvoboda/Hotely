using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("WebApi/ReservedRoomList")]
    public class ReservedRoomsApi : ControllerBase {

        [HttpGet("/WebApi/ReservedRoomList/{hotelId}/{startDate}/{endDate}/{language}")]
        public async Task<string> GetReservedRoomsApi(int hotelId, DateTime startDate, DateTime endDate, string language = "cz") {
            List<HotelReservedRoomList> result;

            int bookedCalendarMonthsCountBefore;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    bookedCalendarMonthsCountBefore = int.Parse(new hotelsContext().WebSettingLists.Where(a => a.Key == "BookCalendarViewMonthsBefore").FirstOrDefault().Value);
                }
            } catch { bookedCalendarMonthsCountBefore = 1; }

            int bookedCalendarMonthsCountAfter;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    bookedCalendarMonthsCountAfter = int.Parse(new hotelsContext().WebSettingLists.Where(a => a.Key == "BookCalendarViewMonthsAfter").FirstOrDefault().Value);
                }
            } catch { bookedCalendarMonthsCountAfter = 12; }

            //set bookingcalendarshowrange
            if (bookedCalendarMonthsCountBefore == 1) { startDate = startDate.AddDays(1 - startDate.Day); } else { startDate = startDate.AddDays(1 - startDate.Day); startDate.AddMonths(-bookedCalendarMonthsCountBefore); }
            endDate = endDate.AddDays(-startDate.Day); endDate = endDate.AddMonths(bookedCalendarMonthsCountAfter);

            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                result = new hotelsContext().HotelReservedRoomLists
                    .Where(a => 
                    a.HotelId == hotelId && (a.StatusId == 2 || a.StatusId == 5) &&
                    ((a.StartDate <= startDate && a.EndDate > startDate) 
                    || (a.StartDate >= startDate && a.EndDate <= endDate)
                    || (a.StartDate < endDate && a.EndDate >= endDate)
                    ))
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