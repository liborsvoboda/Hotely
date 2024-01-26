namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("HotelList")]
    public class HotelListApi : ControllerBase {

        [HttpGet("/HotelList")]
        public async Task<string> GetHotelList() {
            List<HotelList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower()))
                { data = new hotelsContext().HotelLists.Include(a => a.City).ToList(); }
                else
                {
                    data = new hotelsContext().HotelLists.Include(a => a.City).Include(a => a.User)
                        .Where(a => a.User.UserName == Request.HttpContext.User.Claims.First().Issuer).ToList();
                }
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/HotelList/Filter/{filter}")]
        public async Task<string> GetHotelListByFilter(string filter) {
            List<HotelList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower()))
                { data = new hotelsContext().HotelLists.FromSqlRaw("SELECT * FROM HotelList WHERE 1=1 AND " + filter.Replace("+", " "))
                        .Include(a => a.City).AsNoTracking().ToList(); }
                else
                {
                    data = new hotelsContext().HotelLists.FromSqlRaw("SELECT * FROM HotelList WHERE 1=1 AND " + filter.Replace("+", " "))
                        .Include(a => a.City).Include(a => a.User).Where(a => a.User.UserName == Request.HttpContext.User.Claims.First().Issuer)
                        .AsNoTracking().ToList();
                }
            }
            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/HotelList/Active")]
        public async Task<string> GetActiveHotel() {
            HotelList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                data = new hotelsContext().HotelLists
                     .Include(a => a.City).Include(a => a.User).Where(a => a.User.UserName == Request.HttpContext.User.Claims.First().Issuer).First();
            }
            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/HotelList/{id}")]
        public async Task<string> GetHotelListKey(int id) {
            HotelList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            { data = new hotelsContext().HotelLists.Include(a => a.City).Where(a => a.Id == id).First(); }
            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/HotelList")]
        [Consumes("application/json")]
        public async Task<string> InsertHotelList([FromBody] HotelList record) {
            try
            {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().HotelLists.Add(record);
                int result = await data.Context.SaveChangesAsync();

                //Create Properties for new Hotel
                List<SqlParameter> parameters = new();
                parameters = new List<SqlParameter> {
                        new SqlParameter { ParameterName = "@HotelId", Value = record.Id },
                        new SqlParameter { ParameterName = "@PropertyId", IsNullable = true, DbType = System.Data.DbType.Int32, Value = DBNull.Value },
                        };
                new hotelsContext().Database.ExecuteSqlRaw("exec GenerateHotelProperties @HotelId, @PropertyId", parameters.ToArray()).ToString();

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/HotelList")]
        [Consumes("application/json")]
        public async Task<string> UpdateHotelList([FromBody] HotelList record) {
            try
            {
                var data = new hotelsContext().HotelLists.Update(record);
                int result = await data.Context.SaveChangesAsync();

                //Check Properties for updated Hotel
                List<SqlParameter> parameters = new();
                parameters = new List<SqlParameter> {
                        new SqlParameter { ParameterName = "@HotelId", Value = record.Id },
                        new SqlParameter { ParameterName = "@PropertyId", IsNullable = true, DbType = System.Data.DbType.Int32, Value = DBNull.Value },
                        };
                new hotelsContext().Database.ExecuteSqlRaw("exec GenerateHotelProperties @HotelId, @PropertyId", parameters.ToArray()).ToString();

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/HotelList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteHotelList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                HotelList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().HotelLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();

                //Remove Item Attachments Previous delete Item HERE is not deleted BY foreign key
                List<HotelImagesList> ImagesData;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                { ImagesData = new hotelsContext().HotelImagesLists.Where(a => a.HotelId == int.Parse(id)).ToList(); }
                var itemData = new hotelsContext(); itemData.HotelImagesLists.RemoveRange(ImagesData);
                itemData.SaveChanges();

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