namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("MessageModuleList")]
    public class MessageModuleListApi : ControllerBase {

        [HttpGet("/MessageModuleList")]
        public async Task<string> GetMessageModuleList() {
            List<MessageModuleList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().MessageModuleLists.OrderBy(a => a.IsSystemMessage).ThenBy(a=>a.Shown).ToList();
            }
            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/MessageModuleList/Filter/{filter}")]
        public async Task<string> GetMessageModuleListByFilter(string filter) {
            List<MessageModuleList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                data = new hotelsContext().MessageModuleLists.FromSqlRaw("SELECT * FROM MessageModuleList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/MessageModuleList/{id}")]
        public async Task<string> GetMessageModuleListKey(int id) {
            MessageModuleList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }))
            {
                data = new hotelsContext().MessageModuleLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/MessageModuleList")]
        [Consumes("application/json")]
        public async Task<string> InsertMessageModuleList([FromBody] MessageModuleList record) {
            try {
                var data = new hotelsContext().MessageModuleLists.Add(record);
                int result = await data.Context.SaveChangesAsync();


                //Send MessageModule to Email
                if (record.Published) {
                    MessageModuleList loadRecordData = null;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        loadRecordData = new hotelsContext().MessageModuleLists.Include(a => a.MessageType).Where(a => a.Id == record.Id).FirstOrDefault();
                    }

                    if (loadRecordData.MessageType.Name.ToLower() == "newsletter") {
                        var guestList = new hotelsContext().GuestLists.Include(a => a.GuestSettingLists).Where(a => a.GuestSettingLists.Where(a => a.Key == "sendNewsletterToEmail" && a.Value == "true").Any()).ToList();
                        guestList.ForEach(guest => { ServerCoreFunctions.SendEmail(new MailRequest() { Sender = ServerConfigSettings.EmailerBusinessEmailAddress, Recipients = new List<string> { guest.Email }, Subject = record.Subject, Content = record.HtmlMessage }); });
                    }
                    else if (loadRecordData.MessageType.Name.ToLower() != "newsletter") {
                        var guest = new hotelsContext().GuestLists.Include(a => a.GuestSettingLists).Where(a => a.Id == record.GuestId && a.GuestSettingLists.Where(a => a.Key == "sendNewMessagesToEmail" && a.Value == "true").Any()).FirstOrDefault();
                        if (guest != null) { ServerCoreFunctions.SendEmail(new MailRequest() { Sender = ServerConfigSettings.EmailerBusinessEmailAddress, Recipients = new List<string> { guest.Email }, Subject = record.Subject, Content = record.HtmlMessage }); }
                    }
                }


                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/MessageModuleList")]
        [Consumes("application/json")]
        public async Task<string> UpdateMessageModuleList([FromBody] MessageModuleList record) {
            try {
                var data = new hotelsContext().MessageModuleLists.Update(record);
                int result = await data.Context.SaveChangesAsync();

                //Send MessageModule to Email
                if (record.Published) {
                    MessageModuleList loadRecordData = null;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        loadRecordData = new hotelsContext().MessageModuleLists.Include(a => a.MessageType).Where(a => a.Id == record.Id).FirstOrDefault();
                    }

                    if (loadRecordData.MessageType.Name.ToLower() == "newsletter") {
                        var guestList = new hotelsContext().GuestLists.Include(a => a.GuestSettingLists).Where(a => a.GuestSettingLists.Where(a => a.Key == "sendNewsletterToEmail" && a.Value == "true").Any()).ToList();
                        guestList.ForEach(guest => { ServerCoreFunctions.SendEmail(new MailRequest() { Sender = ServerConfigSettings.EmailerBusinessEmailAddress, Recipients = new List<string> { guest.Email }, Subject = record.Subject, Content = record.HtmlMessage }); });
                    }
                    else if (loadRecordData.MessageType.Name.ToLower() != "newsletter") {
                        var guest = new hotelsContext().GuestLists.Include(a => a.GuestSettingLists).Where(a => a.Id == record.GuestId && a.GuestSettingLists.Where(a => a.Key == "sendNewMessagesToEmail" && a.Value == "true").Any()).FirstOrDefault();
                        if (guest != null) { ServerCoreFunctions.SendEmail(new MailRequest() { Sender = ServerConfigSettings.EmailerBusinessEmailAddress, Recipients = new List<string> { guest.Email }, Subject = record.Subject, Content = record.HtmlMessage }); }
                    }
                }

                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/MessageModuleList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteMessageModuleList(int id) {
            try
            {
                MessageModuleList record = new() { Id = id };

                var data = new hotelsContext().MessageModuleLists.Remove(record);
                int result = await data.Context.SaveChangesAsync();
                if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) });
            }
        }
    }
}