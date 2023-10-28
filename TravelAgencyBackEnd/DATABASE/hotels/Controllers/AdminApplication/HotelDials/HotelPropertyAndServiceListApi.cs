namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("HotelPropertyAndServiceList")]
    public class HotelPropertyAndServiceListApi : ControllerBase {

        [HttpGet("/HotelPropertyAndServiceList")]
        public async Task<string> GetHotelPropertyAndServiceList() {
            List<HotelPropertyAndServiceList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                if (Request.HttpContext.User.IsInRole("Admin")) {
                    data = new hotelsContext().HotelPropertyAndServiceLists
                        .Include(a=> a.PropertyOrService)
                        .OrderBy(a => a.PropertyOrService.PropertyGroupId)
                        .ToList()
                        .OrderBy(a => a.HotelId).ToList()
                        ; 
                } else {
                    data = new hotelsContext().HotelPropertyAndServiceLists.Include(a => a.User)
                        .Where(a => a.User.UserName == Request.HttpContext.User.Claims.First().Issuer)
                        .Include(a => a.PropertyOrService)
                        .OrderBy(a => a.PropertyOrService.PropertyGroupId)
                        .ToList()
                        .OrderBy(a => a.HotelId).ToList()
                        ;
                }
            }

            data.ForEach(item => { item.PropertyOrService = null; });

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/HotelPropertyAndServiceList/Filter/{filter}")]
        public async Task<string> GetHotelPropertyAndServiceListByFilter(string filter) {
            List<HotelPropertyAndServiceList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                if (Request.HttpContext.User.IsInRole("Admin"))
                { data = new hotelsContext().HotelPropertyAndServiceLists.FromSqlRaw("SELECT * FROM HotelPropertyAndServiceList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList(); }
                else
                {
                    data = new hotelsContext().HotelPropertyAndServiceLists.FromSqlRaw("SELECT * FROM HotelPropertyAndServiceList WHERE 1=1 AND " + filter.Replace("+", " "))
                        .Include(a => a.User).Where(a => a.User.UserName == Request.HttpContext.User.Claims.First().Issuer)
                        .AsNoTracking().ToList();
                }
            }
            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/HotelPropertyAndServiceList/Active")]
        public async Task<string> GetActiveHotel() {
            HotelPropertyAndServiceList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                data = new hotelsContext().HotelPropertyAndServiceLists
                     .Include(a => a.User).Where(a => a.User.UserName == Request.HttpContext.User.Claims.First().Issuer).First();
            }
            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/HotelPropertyAndServiceList/{id}")]
        public async Task<string> GetHotelPropertyAndServiceListKey(int id) {
            HotelPropertyAndServiceList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            { data = new hotelsContext().HotelPropertyAndServiceLists.Where(a => a.Id == id).First(); }
            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/HotelPropertyAndServiceList")]
        [Consumes("application/json")]
        public async Task<string> InsertHotelPropertyAndServiceList([FromBody] HotelPropertyAndServiceList record) {
            try
            {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().HotelPropertyAndServiceLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/HotelPropertyAndServiceList")]
        [Consumes("application/json")]
        public async Task<string> UpdateHotelPropertyAndServiceList([FromBody] HotelPropertyAndServiceList record) {
            try
            {
                var data = new hotelsContext().HotelPropertyAndServiceLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/HotelPropertyAndServiceList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteHotelPropertyAndServiceList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                HotelPropertyAndServiceList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().HotelPropertyAndServiceLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) });
            }
        }
    }
}