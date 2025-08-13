namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("BasicImageGalleryList")]
    public class BasicImageGalleryListApi : ControllerBase {

        [HttpGet("/BasicImageGalleryList")]
        public async Task<string> GetBasicImageGalleryList() {
            List<BasicImageGalleryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                if (ServerApiServiceExtension.IsAdmin()) { data = new hotelsContext().BasicImageGalleryLists.ToList(); }
                else {
                    data = new hotelsContext().BasicImageGalleryLists.Include(a => a.User)
                        .Where(a => a.User.UserName == Request.HttpContext.User.Claims.First().Issuer).ToList();
                }
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/BasicImageGalleryList/Filter/{filter}")]
        public async Task<string> GetBasicImageGalleryListByFilter(string filter) {
            List<BasicImageGalleryList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                if (ServerApiServiceExtension.IsAdmin()) { data = new hotelsContext().BasicImageGalleryLists.FromSqlRaw("SELECT * FROM BasicImageGalleryList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList(); }
                else {
                    data = new hotelsContext().BasicImageGalleryLists.FromSqlRaw("SELECT * FROM BasicImageGalleryList WHERE 1=1 AND " + filter.Replace("+", " "))
                        .Include(a => a.User).Where(a => a.User.UserName == Request.HttpContext.User.Claims.First().Issuer)
                        .AsNoTracking().ToList();
                }
            }
            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/BasicImageGalleryList/{id}")]
        public async Task<string> GetBasicImageGalleryListById(int id) {
            BasicImageGalleryList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) { data = new hotelsContext().BasicImageGalleryLists.Where(a => a.Id == id).First(); }
            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/BasicImageGalleryList")]
        [Consumes("application/json")]
        public async Task<string> InsertBasicImageGalleryList([FromBody] BasicImageGalleryList record) {
            try {
                record.User = null;  //EntityState.Detached IDENTITY_INSERT is set to OFF
                var data = new hotelsContext().BasicImageGalleryLists.Add(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/BasicImageGalleryList")]
        [Consumes("application/json")]
        public async Task<string> UpdateBasicImageGalleryList([FromBody] BasicImageGalleryList record) {
            try {
                var data = new hotelsContext().BasicImageGalleryLists.Update(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/BasicImageGalleryList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteBasicImageGalleryList(string id) {
            try {
                if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                BasicImageGalleryList record = new() { Id = int.Parse(id) };

                var data = new hotelsContext().BasicImageGalleryLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}