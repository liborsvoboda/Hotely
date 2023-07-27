using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("WebApi/Guest")]
    public class WebRegistrationApi : ControllerBase {

        [HttpPost("/WebApi/Guest/ResetPassword")]
        [Consumes("application/json")]
        public IActionResult PostResetPassword([FromBody] AutoGenEmailAddress record) {
            try {
                if (!string.IsNullOrWhiteSpace(record.EmailAddress) && Functions.IsValidEmail(record.EmailAddress)) {
                    string newPassword = Functions.RandomString(10);

                    //check email exist
                    GuestList data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        data = new hotelsContext().GuestLists.Where(a => a.Email == record.EmailAddress && a.Active).FirstOrDefault();
                    }
                    if (data == null) {
                        return BadRequest(JsonSerializer.Serialize(new DBResultMessage() {
                            Status = DBWebApiResponses.emailNotExist.ToString(),
                            ErrorMessage = DBOperations.DBTranslate(DBWebApiResponses.emailNotExist.ToString(), record.Language)
                        }));
                    }

                    //Set new Password
                    data.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    var dbdata = new hotelsContext().GuestLists.Update(data);
                    dbdata.Context.SaveChanges();


                    //Send ResetPassword Email
                    EmailTemplateList template = new hotelsContext().EmailTemplateLists.Where(a => a.TemplateName == "resetPassword").FirstOrDefault();
                    MailRequest mailRequest = new MailRequest();
                    if (template != null) {
                        mailRequest = new MailRequest() {
                            Subject = (record.Language == "cz" ? template.SubjectCz : template.SubjectEn)
                            .Replace("[firstname]", data.FirstName).Replace("[lastname]", data.LastName)
                            .Replace("[password]", newPassword).Replace("[email]", record.EmailAddress),
                            Recipients = new List<string>() { record.EmailAddress },
                            Content = (record.Language == "cz" ? template.EmailCz : template.EmailEn)
                            .Replace("[firstname]", data.FirstName).Replace("[lastname]", data.LastName)
                            .Replace("[password]", newPassword).Replace("[email]", record.EmailAddress)
                        };
                    }
                    else {
                        mailRequest = new() {
                            Subject = "Úbytkač New Password Email",
                            Recipients = new() { record.EmailAddress },
                            Content = "Your new password for login is: " + newPassword + Environment.NewLine
                        };
                    }
                    string result = SystemFunctions.SendEmail(mailRequest);


                    if (result == DBResult.success.ToString()) { return Ok(JsonSerializer.Serialize(new { message = DBResult.success.ToString() })); } else { return BadRequest(JsonSerializer.Serialize(result)); }
                }
                else { return BadRequest(new { message = DBOperations.DBTranslate("EmailAddressIsNotValid", record.Language) }); }
            } catch { }
            return BadRequest(new { message = DBOperations.DBTranslate("EmailCannotBeSend", record.Language) });
        }

        [HttpPost("/WebApi/Guest/SendVerifyCode")]
        [Consumes("application/json")]
        public IActionResult PostSendVerifyCode([FromBody] AutoGenEmailAddress record) {
            try {
                if (!string.IsNullOrWhiteSpace(record.EmailAddress) && Functions.IsValidEmail(record.EmailAddress)) {
                    string verifyCode = Functions.RandomString(10);

                    //check email exist
                    int count;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        count = new hotelsContext().GuestLists.Where(a => a.Email == record.EmailAddress && a.Active).Count();
                    }
                    if (count > 0) {
                        return BadRequest(JsonSerializer.Serialize(new DBResultMessage() {
                            Status = DBWebApiResponses.emailExist.ToString(),
                            ErrorMessage = DBOperations.DBTranslate(DBWebApiResponses.emailExist.ToString(), record.Language)
                        }));
                    }

                    //Send Verify Email
                    EmailTemplateList template = new hotelsContext().EmailTemplateLists.Where(a => a.TemplateName == "verification").FirstOrDefault();
                    MailRequest mailRequest = new MailRequest();
                    if (template != null) {
                        mailRequest = new MailRequest() {
                            Subject = (record.Language == "cz" ? template.SubjectCz : template.SubjectEn).Replace("[verifyCode]", verifyCode),
                            Recipients = new List<string>() { record.EmailAddress },
                            Content = (record.Language == "cz" ? template.EmailCz : template.EmailEn).Replace("[verifyCode]", verifyCode)
                        };
                    }
                    else {
                        mailRequest = new() {
                            Subject = "Úbytkač Registration Verification Email",
                            Recipients = new() { record.EmailAddress },
                            Content = "Your Registration Verify Code is: " + verifyCode + Environment.NewLine
                        };
                    }
                    string result = SystemFunctions.SendEmail(mailRequest);


                    if (result == DBResult.success.ToString()) { return Ok(JsonSerializer.Serialize(new { verifyCode = verifyCode })); } else { return BadRequest(JsonSerializer.Serialize(result)); }
                }
                else { return BadRequest(new { message = DBOperations.DBTranslate("EmailAddressIsNotValid", record.Language) }); }
            } catch { }
            return BadRequest(new { message = DBOperations.DBTranslate("EmailCannotBeSend", record.Language) });
        }


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
                int result;

                //checkDeactivated
                GuestList origUser = new();
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    origUser = new hotelsContext().GuestLists.Where(a => a.Email == record.User.Email).FirstOrDefault();
                }
                if (origUser != null) { 
                    record.User.Id = origUser.Id;
                    record.User.Timestamp = DateTimeOffset.Now.DateTime;
                    var data = new hotelsContext().GuestLists.Update(record.User);
                    result = await data.Context.SaveChangesAsync();
                }
                else {
                    record.User.Timestamp = DateTimeOffset.Now.DateTime;
                    var data = new hotelsContext().GuestLists.Add(record.User);
                    result = await data.Context.SaveChangesAsync();
                }


                //Send Reg Email
                EmailTemplateList template = new hotelsContext().EmailTemplateLists.Where(a=>a.TemplateName == "registration").FirstOrDefault();
                MailRequest mailRequest = new MailRequest();
                if (template != null) {
                    mailRequest = new MailRequest() {
                        Subject = (record.Language == "cz" ? template.SubjectCz : template.SubjectEn)
                        .Replace("[firstname]", record.User.FirstName).Replace("[lastname]", record.User.LastName)
                        .Replace("[email]", record.User.Email).Replace("[password]", origPassword),
                        Recipients = new List<string>() { record.User.Email },
                        Content = (record.Language == "cz" ? template.EmailCz : template.EmailEn)
                        .Replace("[firstname]", record.User.FirstName).Replace("[lastname]", record.User.LastName)
                        .Replace("[email]", record.User.Email).Replace("[password]", origPassword),
                    };
                } else {
                    mailRequest = new MailRequest() {
                        Subject = "Úbytkač Registration Email",
                        Recipients = new List<string>() { record.User.Email },
                        Content = "Registration for " + record.User.Email + Environment.NewLine + " with password " + origPassword
                    }; 
                }
                SystemFunctions.SendEmail(mailRequest);

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.User.Id, Status = DBWebApiResponses.loginInfoSentToEmail.ToString(), RecordCount = result, ErrorMessage = DBOperations.DBTranslate(DBWebApiResponses.loginInfoSentToEmail.ToString(), record.Language) });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [Authorize]
        [HttpPost("/WebApi/Guest/UpdateRegistration")]
        [Consumes("application/json")]
        public async Task<string> UpdateRegistration([FromBody] GuestRegistration record) {
            try {
                if (User.Claims.First(a => a.Issuer != null).Issuer.ToLower() == record.User.Email.ToLower()) {

                    record.User.Password = BCrypt.Net.BCrypt.HashPassword(record.User.Password);
                    record.User.Timestamp = DateTimeOffset.Now.DateTime;
                    var data = new hotelsContext().GuestLists.Update(record.User);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.User.Id, Status = DBWebApiResponses.loginInfoSentToEmail.ToString(), RecordCount = result, ErrorMessage = DBOperations.DBTranslate(DBWebApiResponses.loginInfoSentToEmail.ToString(), record.Language) });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });

                } else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }

        [Authorize]
        [HttpDelete("/WebApi/Guest/DeleteRegistration")]
        [Consumes("application/json")]
        public async Task<string> DeleteRegistration() {
            try {

                //check authorized Deactivation
                GuestList origUser = new();
                string authEmail = User.Claims.First(a => a.Issuer != null).Issuer.ToLower();
                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    origUser = new hotelsContext().GuestLists.Where(a => a.Email.ToLower() == authEmail && a.Id == int.Parse(authId)).FirstOrDefault();
                }

                if (origUser != null) {
                    origUser.Active = false;
                    origUser.Timestamp = DateTimeOffset.Now.DateTime;
                    var data = new hotelsContext().GuestLists.Update(origUser);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = origUser.Id, Status = DBWebApiResponses.loginInfoSentToEmail.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });

                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }
    }
}