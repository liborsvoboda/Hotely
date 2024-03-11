namespace UbytkacBackend.ServerCoreDBSettings {

    [ApiController]
    [Route("/WebApi/WebUser")]
     //[ApiExplorerSettings(IgnoreApi = true)]
    public class WebPagesUserApi : ControllerBase {

        /// <summary>
        /// WebPages Verification Email
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        [HttpPost("/WebApi/WebUser/SendVerifyCode")]
        [Consumes("application/json")]
        public async Task<IActionResult> PostSendVerifyCode([FromBody] EmailVerification record) {
            try {
                if (!string.IsNullOrWhiteSpace(record.EmailAddress) && DataOperations.IsValidEmail(record.EmailAddress)) {
                    string verifyCode = DataOperations.RandomString(10);

                    //check email exist
                    int count;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        count = new hotelsContext().SolutionUserLists.Where(a => a.UserName == record.EmailAddress && a.Active).Count();
                    }
                    if (count > 0) {
                        return BadRequest(JsonSerializer.Serialize(new DBResultMessage() {
                            Status = DBWebApiResponses.emailExist.ToString(),
                            ErrorMessage = DbOperations.DBTranslate(DBWebApiResponses.emailExist.ToString(), record.Language)
                        }));
                    }

                    //Send Verify Email
                    SolutionEmailTemplateList template = new hotelsContext().SolutionEmailTemplateLists.Where(a => a.TemplateName.ToLower() == "verification" && a.SystemLanguage.SystemName.ToLower() == record.Language.ToLower()).FirstOrDefault();
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
                            Subject = "Groupware-Solution.Eu Verification Email",
                            Recipients = new() { record.EmailAddress },
                            Content = "Your Registration Verify Code is: " + verifyCode + Environment.NewLine
                        };
                    }
                    string result = CoreOperations.SendEmail(mailRequest, true);

                    if (result == DBResult.success.ToString()) { return Ok(JsonSerializer.Serialize(new { verifyCode = verifyCode })); } else { return BadRequest(JsonSerializer.Serialize(result)); }
                }
                else { return BadRequest(new { message = DbOperations.DBTranslate("EmailAddressIsNotValid", ServerConfigSettings.ServiceServerLanguage) }); }
            } catch { }
            return BadRequest(new { message = DbOperations.DBTranslate("EmailCannotBeSend", ServerConfigSettings.ServiceServerLanguage) });
        }

        [HttpPost("/WebApi/WebUser/Registration")]
        [Consumes("application/json")]
        public async Task<string> PostRegistration([FromBody] WebRegistration record) {
            try {
                //check email exist
                int count;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    count = new hotelsContext().SolutionUserLists.Where(a => a.UserName == record.EmailAddress && a.Active).Count();
                }
                if (count > 0) {
                    return JsonSerializer.Serialize(new DBResultMessage() {
                        Status = DBWebApiResponses.emailExist.ToString(),
                        ErrorMessage = DbOperations.DBTranslate(DBWebApiResponses.emailExist.ToString(), record.Language)
                    });
                }

                //checkDeactivated
                int result;
                SolutionUserList origUser = new();
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    origUser = new hotelsContext().SolutionUserLists.Where(a => a.UserName == record.EmailAddress).FirstOrDefault();
                }
                if (origUser != null) {
                    origUser.Password = record.Password;
                    origUser.Active = true;
                    origUser.Timestamp = DateTimeOffset.Now.DateTime;
                    var data = new hotelsContext().SolutionUserLists.Update(origUser);
                    result = await data.Context.SaveChangesAsync();
                }
                else {
                    origUser = new() { RoleId = 4, UserName = record.EmailAddress, Password = record.Password, Name = record.EmailAddress, SurName = record.EmailAddress, Active = true, Timestamp = DateTimeOffset.Now.DateTime };
                    var data = new hotelsContext().SolutionUserLists.Add(origUser);
                    result = await data.Context.SaveChangesAsync();
                }

                //Send Reg Email
                SolutionEmailTemplateList template = new hotelsContext().SolutionEmailTemplateLists.Where(a => a.TemplateName.ToLower() == "registration" && a.SystemLanguage.SystemName.ToLower() == record.Language.ToLower()).FirstOrDefault();
                MailRequest mailRequest = new MailRequest();
                if (template != null) {
                    mailRequest = new MailRequest() {
                        Subject = template.Subject
                        .Replace("[email]", record.EmailAddress).Replace("[password]", record.Password),
                        Recipients = new List<string>() { record.EmailAddress },
                        Content = template.Email
                        .Replace("[email]", record.EmailAddress).Replace("[password]", record.Password),
                    };
                }
                else {
                    mailRequest = new MailRequest() {
                        Subject = "GroupWare-Solution.Eu Registration Email",
                        Recipients = new List<string>() { record.EmailAddress },
                        Content = "Registration for " + record.EmailAddress + Environment.NewLine + " with password " + record.Password
                    };
                }
                CoreOperations.SendEmail(mailRequest, true);
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = origUser.Id, Status = DBWebApiResponses.loginInfoSentToEmail.ToString(), RecordCount = result, ErrorMessage = DbOperations.DBTranslate(DBWebApiResponses.loginInfoSentToEmail.ToString(), record.Language) });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            } catch { }
            return JsonSerializer.Serialize(new { message = DbOperations.DBTranslate("EmailCannotBeSend", ServerConfigSettings.ServiceServerLanguage) });
        }

        [HttpPost("/WebApi/WebUser/ResetPassword")]
        [Consumes("application/json")]
        public IActionResult PostResetPassword([FromBody] EmailVerification record) {
            try {
                if (!string.IsNullOrWhiteSpace(record.EmailAddress) && DataOperations.IsValidEmail(record.EmailAddress)) {
                    string newPassword = DataOperations.RandomString(10);

                    //check email exist
                    SolutionUserList data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        data = new hotelsContext().SolutionUserLists.Where(a => a.UserName == record.EmailAddress && a.Active).FirstOrDefault();
                    }
                    if (data == null) {
                        return BadRequest(JsonSerializer.Serialize(new DBResultMessage() {
                            Status = DBWebApiResponses.emailNotExist.ToString(),
                            ErrorMessage = DbOperations.DBTranslate(DBWebApiResponses.emailNotExist.ToString(), record.Language)
                        }));
                    }

                    //Set new Password
                    data.Password = newPassword;
                    var dbdata = new hotelsContext().SolutionUserLists.Update(data);
                    dbdata.Context.SaveChanges();

                    //Send ResetPassword Email
                    SolutionEmailTemplateList template = new hotelsContext().SolutionEmailTemplateLists.Where(a => a.TemplateName.ToLower() == "resetPassword" && a.SystemLanguage.SystemName.ToLower() == record.Language.ToLower()).FirstOrDefault();
                    MailRequest mailRequest = new MailRequest();
                    if (template != null) {
                        mailRequest = new MailRequest() {
                            Subject = template.Subject
                            .Replace("[password]", newPassword).Replace("[email]", record.EmailAddress),
                            Recipients = new List<string>() { record.EmailAddress },
                            Content = template.Email
                            .Replace("[password]", newPassword).Replace("[email]", record.EmailAddress)
                        };
                    }
                    else {
                        mailRequest = new() {
                            Subject = "GroupWare-Solution.Eu Reset Password Email",
                            Recipients = new() { record.EmailAddress },
                            Content = "Your new password for login is: " + newPassword + Environment.NewLine
                        };
                    }
                    string result = CoreOperations.SendEmail(mailRequest, true);

                    if (result == DBResult.success.ToString()) { return Ok(JsonSerializer.Serialize(new { message = DBResult.success.ToString() })); } else { return BadRequest(JsonSerializer.Serialize(result)); }
                }
                else { return BadRequest(new { message = DbOperations.DBTranslate("EmailAddressIsNotValid", record.Language) }); }
            } catch { }
            return BadRequest(new { message = DbOperations.DBTranslate("EmailCannotBeSend", record.Language) });
        }

        [Authorize]
        [HttpPost("/WebApi/WebUser/UpdateRegistration")]
        [Consumes("application/json")]
        public async Task<string> UpdateRegistration([FromBody] UserProfile record) {
            try {
                if (User.Claims.First(a => a.Issuer != null).Issuer.ToLower() == record.EmailAddress.ToLower()) {
                    string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                    SolutionUserList user = new hotelsContext().SolutionUserLists.Where(a => a.Id == int.Parse(authId)).First();
                    if (record.Password != null && record.Password.Length > 0) { user.Password = record.Password; }
                    user.Name = record.Firstname;
                    user.SurName = record.Lastname;
                    user.Timestamp = DateTimeOffset.Now.DateTime;

                    var data = new hotelsContext().SolutionUserLists.Update(user);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = user.Id, Status = DBWebApiResponses.loginInfoSentToEmail.ToString(), RecordCount = result, ErrorMessage = DBWebApiResponses.loginInfoSentToEmail.ToString() });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [Authorize]
        [HttpGet("/WebApi/WebUser/GetWebUser/{language}")]
        public async Task<string> GetWebUser(string language = "cz") {
            try {
                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                SolutionUserList result;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    result = new hotelsContext().SolutionUserLists.Where(a => a.Id == int.Parse(authId)).First();
                }

                return JsonSerializer.Serialize(result, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }
    }
}