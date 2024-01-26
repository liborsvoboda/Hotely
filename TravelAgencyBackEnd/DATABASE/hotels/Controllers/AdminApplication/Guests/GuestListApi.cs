namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("GuestList")]
    public class GuestListApi : ControllerBase {

        [HttpGet("/GuestList")]
        public async Task<string> GetGuestList() {
            List<GuestList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().GuestLists.ToList();
            }
            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/GuestList/Filter/{filter}")]
        public async Task<string> GetGuestListByFilter(string filter) {
            List<GuestList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            }))
            {
                data = new hotelsContext().GuestLists.FromSqlRaw("SELECT * FROM GuestList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpGet("/GuestList/{id}")]
        public async Task<string> GetGuestListKey(int id) {
            GuestList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().GuestLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        [HttpPut("/GuestList")]
        [Consumes("application/json")]
        public async Task<string> InsertGuestList([FromBody] GuestList record) {
            try
            {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    var data = new hotelsContext().GuestLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpPost("/GuestList")]
        [Consumes("application/json")]
        public async Task<string> UpdateGuestList([FromBody] GuestList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    GuestList existingGuest = new GuestList();
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        existingGuest = new hotelsContext().GuestLists.Where(a => a.Id == record.Id).First();
                    }

                    UserList existingUser = null;
                    if (record.UserId != null || existingGuest.UserId != null) {
                        using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                            existingUser = new hotelsContext().UserLists.Where(a => a.Id == ((int?)record.UserId) || a.Id == existingGuest.UserId).First();
                        }
                    }

                    //Degrading acount Disable ystem account
                    if(record.UserId == null || existingGuest.UserId != null) {
                        existingUser.Active = false;
                        var userData = new hotelsContext().UserLists.Update(existingUser); await userData.Context.SaveChangesAsync();
                        existingUser = null; 
                    }

                    //Update Advertiser Password
                    if (existingUser != null && record.Password != existingGuest.Password) { existingUser.Password = record.Password; }
                    //Update All Advertiser/User Data
                    if (existingUser != null) {
                        existingUser.UserName = record.Email; existingUser.Name = record.FirstName; existingUser.SurName = record.LastName; existingUser.Active = record.Active;
                        var userData = new hotelsContext().UserLists.Update(existingUser); await userData.Context.SaveChangesAsync();
                    }


                    //Update Guest Password
                    if (record.Password != existingGuest.Password) { record.Password = BCrypt.Net.BCrypt.HashPassword(record.Password); }

                    var data = new hotelsContext().GuestLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();


                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }

        [HttpDelete("/GuestList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteGuestList(int id) {
            try
            {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {

                    GuestList record = new() { Id = id };

                    GuestList existingGuest = new GuestList();
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        existingGuest = new hotelsContext().GuestLists.Where(a => a.Id == record.Id).First();
                    }

                    UserList existingUser = null;
                    if (existingGuest.UserId != null) {
                        using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                            existingUser = new hotelsContext().UserLists.Where(a => a.Id == existingGuest.UserId).First();
                        }
                    }

                    //Deleting acount Disable system account
                    if (record.UserId != null || existingGuest.UserId != null) {
                        existingUser.Active = false;
                        var userData = new hotelsContext().UserLists.Update(existingUser); await userData.Context.SaveChangesAsync();
                        existingUser = null;
                    }


                    var data = new hotelsContext().GuestLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
            return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DBResult.DeniedYouAreNotAdmin.ToString() });
        }
    }
}