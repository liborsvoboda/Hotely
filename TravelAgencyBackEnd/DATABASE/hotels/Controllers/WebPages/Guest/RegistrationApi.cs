namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("WebApi/Guest")]
    public class WebRegistrationApi : ControllerBase {

        [HttpPost("/WebApi/Guest/WebRegistration")]
        [Consumes("application/json")]
        public async Task<string> SaveWebRegistration([FromBody] GuestRegistration record) {
            try {
                //check email exist
                int count;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) { 
                    count = new hotelsContext().GuestLists.Where(a => a.Email == record.User.Email && a.Active).Count(); }
                if (count > 0) {
                    return JsonSerializer.Serialize(new DBResultMessage() {
                        Status = DBWebApiResponses.emailExist.ToString(),
                        ErrorMessage = DBOperations.DBTranslate(DBWebApiResponses.emailExist.ToString(), record.Language)
                    });
                }


                string origPassword = record.User.Password;
                record.User.Password = BCrypt.Net.BCrypt.HashPassword(record.User.Password);
                var data = new hotelsContext().GuestLists.Add(record.User);
                int result = await data.Context.SaveChangesAsync();



                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.User.Id, Status = DBWebApiResponses.loginInfoSentToEmail.ToString(), RecordCount = result, ErrorMessage = DBOperations.DBTranslate(DBWebApiResponses.loginInfoSentToEmail.ToString(), record.Language) });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpPost("/WebApi/Guest/UpdateRegistration")]
        [Consumes("application/json")]
        public async Task<string> UpdateRegistration([FromBody] GuestRegistration record) {
            try {

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }
    }
}