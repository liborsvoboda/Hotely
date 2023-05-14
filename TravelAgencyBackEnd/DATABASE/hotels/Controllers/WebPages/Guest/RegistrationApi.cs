namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("WebApi/Guest")]
    public class WebRegistrationApi : ControllerBase {

        [HttpPost("/WebApi/Guest/WebRegistration")]
        [Consumes("application/json")]
        public async Task<string> SaveWebRegistration([FromBody] GuestRegistration record) {
            try
            {
                //check email exist
                int count;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                { count = new hotelsContext().GuestLists.Where(a => a.Email == record.User.Email).Count(); }
                if (count > 0)
                {
                    return JsonSerializer.Serialize(new DBResultMessage()
                    {
                        status = DBWebApiResponses.emailExist.ToString(),
                        message = DBOperations.DBTranslate(DBWebApiResponses.emailExist.ToString(), record.Language)
                    });
                }

                record.User.Password = BCrypt.Net.BCrypt.HashPassword(record.User.Password);
                var data = new hotelsContext().GuestLists.Add(record.User);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { insertedId = record.User.Id, status = DBWebApiResponses.loginInfoSendedOnEmail.ToString(), recordCount = result, message = DBOperations.DBTranslate(DBWebApiResponses.loginInfoSendedOnEmail.ToString(), record.Language) });
                else return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = result, message = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { status = DBResult.error.ToString(), recordCount = 0, message = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }
    }
}