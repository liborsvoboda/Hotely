namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("SolutionMessageModuleList")]
    public class SolutionMessageModuleListApi : ControllerBase {

        [HttpGet("/SolutionMessageModuleList")]
        public async Task<string> GetSolutionMessageModuleList() {
            List<SolutionMessageModuleList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().SolutionMessageModuleLists.OrderBy(a => a.IsSystemMessage).ThenBy(b=>b.Shown).ToList();
            }

            //Must be JsonSerializerOptions Reason is Cycle Join On ParentId
            return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
            });
        }

        [HttpGet("/SolutionMessageModuleList/Filter/{filter}")]
        public async Task<string> GetSolutionMessageModuleListByFilter(string filter) {
            List<SolutionMessageModuleList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().SolutionMessageModuleLists.FromSqlRaw("SELECT * FROM SolutionMessageModuleList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
            });
        }

        [HttpGet("/SolutionMessageModuleList/{id}")]
        public async Task<string> GetSolutionMessageModuleListKey(int id) {
            SolutionMessageModuleList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().SolutionMessageModuleLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
            });
        }

        [HttpPut("/SolutionMessageModuleList")]
        [Consumes("application/json")]
        public async Task<string> InsertSolutionMessageModuleList([FromBody] SolutionMessageModuleList record) {
            try {

                //Set Shown and unArchive on ParentMessage
                if (record.MessageParentId != null) {
                    SolutionMessageModuleList parentMessage;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        parentMessage = new hotelsContext().SolutionMessageModuleLists.Where(a => a.Id == record.MessageParentId && !a.IsSystemMessage).FirstOrDefault();
                    }
                    if (parentMessage != null) {
                        parentMessage.Shown = true; parentMessage.Archived = false;
                        var updateParent = new hotelsContext().SolutionMessageModuleLists.Update(parentMessage);
                        await updateParent.Context.SaveChangesAsync();

                        if (parentMessage.MessageParentId != null ) { record.MessageParentId = parentMessage.MessageParentId; }
                    }
                }
                
                
                var data = new hotelsContext().SolutionMessageModuleLists.Add(record);
                int result = await data.Context.SaveChangesAsync();


                //Send MessageModule to Email
                if (record.Published) {
                    SolutionMessageModuleList loadRecordData = null;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        loadRecordData = new hotelsContext().SolutionMessageModuleLists.Include(a => a.MessageType).Where(a => a.Id == record.Id).FirstOrDefault();
                    }

                    if (loadRecordData.MessageType.Name.ToLower() == "newsletter") {
                        var guestList = new hotelsContext().GuestLists.Include(a => a.GuestSettingLists).Where(a => a.GuestSettingLists.Where(a => a.Key == "sendNewsletterToEmail" && a.Value == "true").Any()).ToList();
                        guestList.ForEach((Action<GuestList>)(guest => {
                            CoreOperations.SendEmail(new SendMailRequest() { 
                                Sender = ServerConfigSettings.ConfigManagerEmailAddress, Recipients = new List<string> { guest.Email }, 
                                Subject = record.Subject, Content = record.HtmlMessage.Replace("[guestname]", guest.FirstName).Replace("[guestsurname]", guest.LastName).Replace("[guestemail]", guest.Email)
                            }); 
                        }));
                    }
                    else if (loadRecordData.MessageType.Name.ToLower() == "private") {
                        var guest = new hotelsContext().GuestLists.Include(a => a.GuestSettingLists).Where(a => a.Id == record.GuestId && a.GuestSettingLists.Where(a => a.Key == "sendNewMessagesToEmail" && a.Value == "true").Any()).FirstOrDefault();
                        if (guest != null) {
                            CoreOperations.SendEmail(new SendMailRequest() {
                                Sender = ServerConfigSettings.ConfigManagerEmailAddress, Recipients = new List<string> { guest.Email },
                                Subject = record.Subject, Content = record.HtmlMessage.Replace("[guestname]", guest.FirstName).Replace("[guestsurname]", guest.LastName).Replace("[guestemail]", guest.Email)
                            });
                        }
                    }
                }


                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/SolutionMessageModuleList")]
        [Consumes("application/json")]
        public async Task<string> UpdateSolutionMessageModuleList([FromBody] SolutionMessageModuleList record) {
            try {
                var data = new hotelsContext().SolutionMessageModuleLists.Update(record);
                int result = await data.Context.SaveChangesAsync();

                //Send MessageModule to Email
                if (record.Published) {
                    SolutionMessageModuleList loadRecordData = null;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        loadRecordData = new hotelsContext().SolutionMessageModuleLists.Include(a => a.MessageType).Where(a => a.Id == record.Id).FirstOrDefault();
                    }

                    if (loadRecordData.MessageType.Name.ToLower() == "newsletter") {
                        var guestList = new hotelsContext().GuestLists.Include(a => a.GuestSettingLists).Where(a => a.GuestSettingLists.Where(a => a.Key == "sendNewsletterToEmail" && a.Value == "true").Any()).ToList();
                        guestList.ForEach((Action<GuestList>)(guest => {
                            CoreOperations.SendEmail(new SendMailRequest() {
                                Sender = ServerConfigSettings.ConfigManagerEmailAddress, Recipients = new List<string> { guest.Email }, 
                                Subject = record.Subject, Content = record.HtmlMessage.Replace("[guestname]", guest.FirstName).Replace("[guestsurname]", guest.LastName).Replace("[guestemail]", guest.Email)
                            });
                        }));
                    }
                    else if (loadRecordData.MessageType.Name.ToLower() != "private") {
                        var guest = new hotelsContext().GuestLists.Include(a => a.GuestSettingLists).Where(a => a.Id == record.GuestId && a.GuestSettingLists.Where(a => a.Key == "sendNewMessagesToEmail" && a.Value == "true").Any()).FirstOrDefault();
                        if (guest != null) { 
                            CoreOperations.SendEmail(new SendMailRequest() { 
                                Sender = ServerConfigSettings.ConfigManagerEmailAddress, Recipients = new List<string> { guest.Email }, 
                                Subject = record.Subject, Content = record.HtmlMessage.Replace("[guestname]", guest.FirstName).Replace("[guestsurname]", guest.LastName).Replace("[guestemail]", guest.Email)
                            }); 
                        }
                    }
                }

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/SolutionMessageModuleList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteSolutionMessageModuleList(int id) {
            try
            {
                SolutionMessageModuleList record = new() { Id = id };

                var data = new hotelsContext().SolutionMessageModuleLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}