namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("PropertyOrServiceTypeList")]
    public class PropertyOrServiceTypeListApi : ControllerBase {

        [HttpGet("/PropertyOrServiceTypeList")]
        public async Task<string> GetPropertyOrServiceTypeList() {
            List<PropertyOrServiceTypeList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().PropertyOrServiceTypeLists.OrderBy(a => a.PropertyGroupId).ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/PropertyOrServiceTypeList/Filter/{filter}")]
        public async Task<string> GetPropertyOrServiceTypeListByFilter(string filter) {
            List<PropertyOrServiceTypeList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().PropertyOrServiceTypeLists.FromSqlRaw("SELECT * FROM PropertyOrServiceTypeList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().OrderBy(a => a.PropertyGroupId).ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/PropertyOrServiceTypeList/{id}")]
        public async Task<string> GetPropertyOrServiceTypeListKey(int id) {
            PropertyOrServiceTypeList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().PropertyOrServiceTypeLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpPut("/PropertyOrServiceTypeList")]
        [Consumes("application/json")]
        public async Task<string> InsertPropertyOrServiceTypeList([FromBody] PropertyOrServiceTypeList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                    var data = new hotelsContext().PropertyOrServiceTypeLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();

                    //Create Property in All Hotels
                    List<SqlParameter> parameters = new();
                    parameters = new List<SqlParameter> {
                        new SqlParameter { ParameterName = "@HotelId", IsNullable = true, DbType = System.Data.DbType.Int32, Value = DBNull.Value },
                        new SqlParameter { ParameterName = "@PropertyId", Value = record.Id },
                        };
                    new hotelsContext().Database.ExecuteSqlRaw("exec GenerateHotelProperties @HotelId, @PropertyId", parameters.ToArray()).ToString();

                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpPost("/PropertyOrServiceTypeList")]
        [Consumes("application/json")]
        public async Task<string> UpdatePropertyOrServiceTypeList([FromBody] PropertyOrServiceTypeList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    var data = new hotelsContext().PropertyOrServiceTypeLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();

                    //Recreate Property in All Hotels
                    List<SqlParameter> parameters = new();
                    parameters = new List<SqlParameter> {
                        new SqlParameter { ParameterName = "@HotelId", IsNullable = true, DbType = System.Data.DbType.Int32, Value = DBNull.Value },
                        new SqlParameter { ParameterName = "@PropertyId", Value = record.Id },
                        };
                    new hotelsContext().Database.ExecuteSqlRaw("exec GenerateHotelProperties @HotelId, @PropertyId", parameters.ToArray()).ToString();

                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpDelete("/PropertyOrServiceTypeList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeletePropertyOrServiceTypeList(string id) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    PropertyOrServiceTypeList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().PropertyOrServiceTypeLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }
    }
}