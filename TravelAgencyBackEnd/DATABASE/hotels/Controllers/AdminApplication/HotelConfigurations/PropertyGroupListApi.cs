namespace TravelAgencyBackEnd.Controllers {

    [Authorize]
    [ApiController]
    [Route("PropertyGroupList")]
    public class PropertyGroupListApi : ControllerBase {

        [HttpGet("/PropertyGroupList")]
        public async Task<string> GetPropertyGroupList() {
            List<PropertyGroupList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().PropertyGroupLists.ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/PropertyGroupList/Filter/{filter}")]
        public async Task<string> GetPropertyGroupListByFilter(string filter) {
            List<PropertyGroupList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().PropertyGroupLists.FromSqlRaw("SELECT * FROM PropertyGroupList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/PropertyGroupList/{id}")]
        public async Task<string> GetPropertyGroupListKey(int id) {
            PropertyGroupList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().PropertyGroupLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpPut("/PropertyGroupList")]
        [Consumes("application/json")]
        public async Task<string> InsertPropertyGroupList([FromBody] PropertyGroupList record) {
            try
            {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().PropertyGroupLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/PropertyGroupList")]
        [Consumes("application/json")]
        public async Task<string> UpdatePropertyGroupList([FromBody] PropertyGroupList record) {
            try
            {
                var data = new hotelsContext().PropertyGroupLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/PropertyGroupList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeletePropertyGroupList(string id) {
            try
            {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = "Id is not set" });

                PropertyGroupList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().PropertyGroupLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.Id, status = DBResult.success.ToString(), recordCount = result, message = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) });
            }
        }
    }
}