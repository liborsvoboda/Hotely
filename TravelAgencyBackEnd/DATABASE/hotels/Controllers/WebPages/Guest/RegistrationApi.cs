using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

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
                            ErrorMessage = ServerCoreDbOperations.DBTranslate(DBWebApiResponses.emailNotExist.ToString(), record.Language)
                        }));
                    }

                    //Set new Password
                    data.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    var dbdata = new hotelsContext().GuestLists.Update(data);
                    dbdata.Context.SaveChanges();


                    //Send ResetPassword Email
                    EmailTemplateList template = new hotelsContext().EmailTemplateLists.Where(a => a.TemplateName.ToLower() == "resetPassword" && a.SystemLanguage.SystemName.ToLower() == record.Language.ToLower()).FirstOrDefault();
                    MailRequest mailRequest = new MailRequest();
                    if (template != null) {
                        mailRequest = new MailRequest() {
                            Subject = template.Subject
                            .Replace("[firstname]", data.FirstName).Replace("[lastname]", data.LastName)
                            .Replace("[password]", newPassword).Replace("[email]", record.EmailAddress),
                            Recipients = new List<string>() { record.EmailAddress },
                            Content = template.Email
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
                    string result = ServerCoreFunctions.SendEmail(mailRequest);


                    if (result == DBResult.success.ToString()) { return Ok(JsonSerializer.Serialize(new { message = DBResult.success.ToString() })); } else { return BadRequest(JsonSerializer.Serialize(result)); }
                }
                else { return BadRequest(new { message = ServerCoreDbOperations.DBTranslate("EmailAddressIsNotValid", record.Language) }); }
            } catch { }
            return BadRequest(new { message = ServerCoreDbOperations.DBTranslate("EmailCannotBeSend", record.Language) });
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
                            ErrorMessage = ServerCoreDbOperations.DBTranslate(DBWebApiResponses.emailExist.ToString(), record.Language)
                        }));
                    }

                    //Send Verify Email
                    EmailTemplateList template = new hotelsContext().EmailTemplateLists.Where(a => a.TemplateName.ToLower() == "verification" && a.SystemLanguage.SystemName.ToLower() == record.Language.ToLower()).FirstOrDefault();
                    MailRequest mailRequest = new MailRequest();
                    if (template != null) {
                        mailRequest = new MailRequest() {
                            Subject = template.Subject.Replace("[verifyCode]", verifyCode),
                            Recipients = new List<string>() { record.EmailAddress },
                            Content = template.Email.Replace("[verifyCode]", verifyCode)
                        };
                    }
                    else {
                        mailRequest = new() {
                            Subject = "Úbytkač Registration Verification Email",
                            Recipients = new() { record.EmailAddress },
                            Content = "Your Registration Verify Code is: " + verifyCode + Environment.NewLine
                        };
                    }
                    string result = ServerCoreFunctions.SendEmail(mailRequest);


                    if (result == DBResult.success.ToString()) { return Ok(JsonSerializer.Serialize(new { verifyCode = verifyCode })); } else { return BadRequest(JsonSerializer.Serialize(result)); }
                }
                else { return BadRequest(new { message = ServerCoreDbOperations.DBTranslate("EmailAddressIsNotValid", record.Language) }); }
            } catch { }
            return BadRequest(new { message = ServerCoreDbOperations.DBTranslate("EmailCannotBeSend", record.Language) });
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
                        ErrorMessage = ServerCoreDbOperations.DBTranslate(DBWebApiResponses.emailExist.ToString(), record.Language)
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

                //prepare DB guest
                GuestList guest = new GuestList() {
                    Email = record.User.Email, Password = record.User.Password != null ? BCrypt.Net.BCrypt.HashPassword(record.User.Password) : origUser.Password, FirstName = record.User.FirstName,
                    LastName = record.User.LastName, Street = record.User.Street, ZipCode = record.User.ZipCode, City = record.User.City,
                    Country = record.User.Country, Phone = record.User.Phone, Active = true, UserId = record.User.UserId, Timestamp = DateTimeOffset.Now.DateTime
                };

                if (origUser != null) { 
                    guest.User.Id = origUser.Id;
                var data = new hotelsContext().GuestLists.Update(guest);
                    result = await data.Context.SaveChangesAsync();
                }
                else {
                    record.User.Timestamp = DateTimeOffset.Now.DateTime;
                    var data = new hotelsContext().GuestLists.Add(guest);
                    result = await data.Context.SaveChangesAsync();
                }


                //Send Reg Email
                EmailTemplateList template = new hotelsContext().EmailTemplateLists.Where(a=>a.TemplateName.ToLower() == "registration" && a.SystemLanguage.SystemName.ToLower() == record.Language.ToLower()).FirstOrDefault();
                MailRequest mailRequest = new MailRequest();
                if (template != null) {
                    mailRequest = new MailRequest() {
                        Subject = template.Subject
                        .Replace("[firstname]", record.User.FirstName).Replace("[lastname]", record.User.LastName)
                        .Replace("[email]", record.User.Email).Replace("[password]", origPassword),
                        Recipients = new List<string>() { record.User.Email },
                        Content = template.Email
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
                ServerCoreFunctions.SendEmail(mailRequest);

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.User.Id, Status = DBWebApiResponses.loginInfoSentToEmail.ToString(), RecordCount = result, ErrorMessage = ServerCoreDbOperations.DBTranslate(DBWebApiResponses.loginInfoSentToEmail.ToString(), record.Language) });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }

        [Authorize]
        [HttpPost("/WebApi/Guest/UpdateRegistration")]
        [Consumes("application/json")]
        public async Task<string> UpdateRegistration([FromBody] GuestRegistration record) {
            try {
                if (User.Claims.First(a => a.Issuer != null).Issuer.ToLower() == record.User.Email.ToLower()) {
                    int result = 0; UserList newUser = new();

                    //prepare DB guest for update
                    GuestList guest = new GuestList() {
                        Id = record.User.Id, Email = record.User.Email, FirstName = record.User.FirstName, LastName = record.User.LastName,
                        Street = record.User.Street, ZipCode = record.User.ZipCode, City = record.User.City, Country = record.User.Country,
                        Phone = record.User.Phone, Active = true, UserId = record.User.UserId, Timestamp = DateTimeOffset.Now.DateTime,
                        Password = BCrypt.Net.BCrypt.HashPassword(record.User.Password)
                    };

                    //insert new systemuser
                    if (record.User.UserId == 0) {
                        newUser = new UserList() { UserName = record.User.Email, RoleId = 5, Password = record.User.Password, Name = record.User.FirstName, SurName = record.User.LastName, Active = true };
                        var insData = new hotelsContext().UserLists.Add(newUser);
                        result = await insData.Context.SaveChangesAsync();
                    }
                    if (result > 0) { guest.UserId = newUser.Id; }


                    //update new systemuser
                    if (record.User.UserId > 0) {
                        UserList systemUser = new hotelsContext().UserLists.Where(a => a.Id == record.User.UserId).FirstOrDefault();

                        guest.Password = record.User.Password != null ? BCrypt.Net.BCrypt.HashPassword(record.User.Password) : BCrypt.Net.BCrypt.HashPassword(systemUser.Password);

                        systemUser.Password = record.User.Password != null ? record.User.Password : systemUser.Password;
                        systemUser.Name = record.User.FirstName; systemUser.SurName = record.User.LastName; systemUser.Active = true;
                        var insData = new hotelsContext().UserLists.Update(systemUser);
                        result = await insData.Context.SaveChangesAsync();
                    }



                    var data = new hotelsContext().GuestLists.Update(guest);
                    result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.User.Id, Status = DBWebApiResponses.loginInfoSentToEmail.ToString(), RecordCount = result, ErrorMessage = ServerCoreDbOperations.DBTranslate(DBWebApiResponses.loginInfoSentToEmail.ToString(), record.Language) });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = ServerCoreDbOperations.DBTranslate(DBWebApiResponses.saveDataError.ToString())  });
                } else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreDbOperations.DBTranslate(DBWebApiResponses.inputDataError.ToString()) });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
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

                    if (origUser.UserId != null) {
                        UserList systemUser = new hotelsContext().UserLists.Where(a => a.Id == origUser.UserId).FirstOrDefault();
                        systemUser.Active = false;
                        var systemData = new hotelsContext().UserLists.Update(systemUser);
                        await systemData.Context.SaveChangesAsync();
                    }

                    origUser.Active = false;
                    origUser.Timestamp = DateTimeOffset.Now.DateTime;
                    var data = new hotelsContext().GuestLists.Update(origUser);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = origUser.Id, Status = DBWebApiResponses.loginInfoSentToEmail.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });

                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }
    }
}