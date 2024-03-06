using UbytkacBackend.MessageModuleClasses;


namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("/WebApi/MessageModule")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MessageModuleApi : ControllerBase {

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public MessageModuleApi(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }

        #region NewsLetter

        [AllowAnonymous]
        [HttpGet("/WebApi/MessageModule/GetNewsLetterList")]
        [Consumes("application/json")]
        public async Task<string> GetNewsLetterList() {
            List<SolutionMessageModuleList> data;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().SolutionMessageModuleLists.Where(a => a.MessageType.Name == "newsletter" && a.Published && !a.Archived).OrderByDescending(a => a.TimeStamp).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        /// <summary>
        /// Message Module NewsLetter Preview API
        /// Saving To index.html
        /// </summary>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpPost("/WebApi/MessageModule/NewsletterPreview")]
        public async Task<string> SaveNewsletterPreview([FromBody] object HtmlContent) {
            try {
                string data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().WebPageSettingLists.Where(a => a.Key == "NewsletterPreviewTemplate").First().Value;
                }
                List<Dictionary<string, string>> htmlData = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(HtmlContent.ToString());
                data = data.Replace("AUTOTITLE", htmlData.Find(a => a.ContainsKey("HtmlTitle")).First().Value);
                data = data.Replace("AUTOCONTENT", htmlData.Find(a => a.ContainsKey("HtmlContent")).First().Value.Split("<BODY>")[1].Split("</BODY>")[0]);

                System.IO.File.WriteAllText(Path.Combine(ServerRuntimeData.Startup_path, "wwwroot", "server-web", "newsletter-preview", "index.html"), data);
                System.IO.File.WriteAllText(Path.Combine(_hostingEnvironment.WebRootPath, "server-web", "newsletter-preview", "index.html"), data);
                return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = 1, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }


        /// <summary>
        /// Message Module NewsLetter Preview API
        /// Redirect todsaved Index.html after POST save
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("/WebApi/MessageModule/NewsletterPreview")]
        public IActionResult GetNewsletterPreview() { return new RedirectResult("/server-web/newsletter-preview/index.html"); }


        #endregion NewsLetter


        #region Web Messages Controls


        /// <summary>
        /// WebApi Get private Messages
        /// </summary>
        /// <returns></returns>
        [HttpGet("/WebApi/MessageModule/GetPrivateMessageList/{archived}")]
        [Consumes("application/json")]
        public async Task<string> GetPrivateMessageList(bool archived) {
            List<SolutionMessageModuleList> data;
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;

                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().SolutionMessageModuleLists
                        .Where(a => a.MessageType.Name == "private" && a.MessageParentId == null && a.Published && ((!archived && !a.Archived) || archived) && a.GuestId == int.Parse(authId) )
                        .Include(a => a.InverseMessageParent)
                        .Include(a => a.MessageType)
                        .OrderByDescending(a => a.TimeStamp).ToList();
                }

                data.ForEach(message => { 
                    message.MessageType.SolutionMessageModuleLists = null;
                    message.InverseMessageParent = message.InverseMessageParent.OrderByDescending(a => a.TimeStamp).ToList();
                });


                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }



        [HttpGet("/WebApi/MessageModule/GetUnreadPrivateMessageCount")]
        [Consumes("application/json")]
        public async Task<string> GetUnreadPrivateMessageCount(bool archived) {
            int unreadPrivateMessages;
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;

                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    unreadPrivateMessages = new hotelsContext().SolutionMessageModuleLists.Where(a => a.MessageType.Name == "private" 
                    && a.IsSystemMessage && !a.Shown && a.GuestId == int.Parse(authId) ).Count();
                }

                return JsonSerializer.Serialize(unreadPrivateMessages, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }



        /// <summary>
        /// WebApi Get Reservation Messages
        /// </summary>
        /// <returns></returns>
        [HttpGet("/WebApi/MessageModule/GetReservationMessageList/{archived}")]
        [Consumes("application/json")]
        public async Task<string> GetReservationMessageList(bool archived) {
            List<SolutionMessageModuleList> data;
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;

                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().SolutionMessageModuleLists.Where(a => a.MessageType.Name == "reservation" && a.GuestId == int.Parse(authId)
                    && a.Published && ((!archived && !a.Archived) || archived))
                        .OrderByDescending(a => a.TimeStamp).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }


        /// <summary>
        /// Guest Set Private Message Answer
        /// </summary>
        /// <param name="messageAnswer"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpPost("/WebApi/MessageModule/SetPrivateMessageAnswer")]
        public async Task<IActionResult> SetPrivateMessageAnswer([FromBody] WebPrivateMessage messageAnswer) {
            try {
                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;

                SolutionMessageModuleList parentMessage;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    parentMessage = new hotelsContext().SolutionMessageModuleLists.Where(a => a.Id == messageAnswer.ParentId && a.GuestId == int.Parse(authId) && a.MessageType.Name == "private").FirstOrDefault();
                }
                if (parentMessage != null) {
                    SolutionMessageModuleList answerMessage = new() { 
                        Level = parentMessage.Level + 1, MessageParentId = messageAnswer.ParentId,MessageTypeId = parentMessage.MessageTypeId, 
                        Subject = DateTimeOffset.UtcNow.ToLocalTime().ToString() + ": " + DbOperations.DBTranslate("AnswerFor", messageAnswer.Language) + ": "+ parentMessage.Subject,
                        HtmlMessage = "<html>\r\n<head>\r\n<meta content=\"text/html;utf-8\" http-equiv=\"content-type\">\r\n</head>\r\n<body>" + messageAnswer.Message + "</body>\r\n</html>",
                        IsSystemMessage = false, Published = true, Shown = false, Archived = false,
                        GuestId = int.Parse(authId), UserId = parentMessage.UserId
                    };

                    var data = new hotelsContext().SolutionMessageModuleLists.Add(answerMessage);
                    int result = await data.Context.SaveChangesAsync();

                    return Ok(JsonSerializer.Serialize( new DBResultMessage() { Status = DBResult.success.ToString(), ErrorMessage = string.Empty }));
                }

            } catch (Exception ex) { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(), ErrorMessage = DbOperations.DBTranslate("PrivateMessageAnswerIsNotValid", messageAnswer.Language)
            });
        }


        /// <summary>
        /// Guest set Archived Root and Sub Private Messages
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpGet("/WebApi/MessageModule/ArchivePrivateMessage/{messageId}/{language}")]
        [Consumes("application/json")]
        public async Task<IActionResult> ArchivePrivateMessage(int messageId, string language = "cz") {
            List<SolutionMessageModuleList> archiveMessage;
            try {
                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    archiveMessage = new hotelsContext().SolutionMessageModuleLists.Where(a => ( a.Id == messageId && a.MessageParentId == null) || ( a.MessageParentId == messageId)
                    && a.GuestId == int.Parse(authId) && !a.Archived && a.MessageType.Name == "private").ToList();
                }
                if (archiveMessage != null) {
                    archiveMessage.ForEach(message => { message.Archived = true; });
                    var data = new hotelsContext();
                    data.SolutionMessageModuleLists.UpdateRange(archiveMessage);
                    await data.SaveChangesAsync();

                    return Ok(JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.success.ToString(), ErrorMessage = string.Empty }));
                }
            } catch (Exception ex) { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DbOperations.DBTranslate("ArchivePrivateMessageRequestIsNotValid", language) });
        }



        /// <summary>
        /// Guest set Shown Private Message By Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpGet("/WebApi/MessageModule/SetShownPrivateMessage/{messageId}/{language}")]
        [Consumes("application/json")]
        public async Task<IActionResult> SetShownPrivateMessage(int messageId, string language = "cz") {
            SolutionMessageModuleList archiveMessage;
            try {
                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    archiveMessage = new hotelsContext().SolutionMessageModuleLists.Where(a => a.Id == messageId && a.GuestId == int.Parse(authId) && !a.Shown && a.MessageType.Name == "private").FirstOrDefault();
                }
                if (archiveMessage != null) {
                    archiveMessage.Shown = true;
                    var updateMessage = new hotelsContext().SolutionMessageModuleLists.Update(archiveMessage);
                    await updateMessage.Context.SaveChangesAsync();

                    return Ok(JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.success.ToString(), ErrorMessage = string.Empty }));
                }
            } catch (Exception ex) { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DbOperations.DBTranslate("ShownPrivateMessageRequestIsNotValid", language) });
        }

        #endregion Web Messages Controls


        #region DiscussionForum


        /// <summary>
        /// Discusion Forum API
        /// </summary>
        /// <param name="archived"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("/WebApi/MessageModule/GetDiscussionForumList/{archived}")]
        [Consumes("application/json")]
        public async Task<string> GetDiscussionForumList(bool archived) {
            List<SolutionMessageModuleList> data;
            try {

                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().SolutionMessageModuleLists
                        .Where(a => a.MessageType.Name.ToLower() == "discussionforum" && a.MessageParentId == null && a.Published && ((!archived && !a.Archived) || archived))
                        .Include(a=>a.MessageType)
                        .Include(a => a.InverseMessageParent).ThenInclude(a => a.Guest)
                        .OrderByDescending(a => a.TimeStamp).ToList();
                }

                data.ForEach(discussion => {
                    discussion.MessageType.SolutionMessageModuleLists = null;
                    discussion.InverseMessageParent = discussion.InverseMessageParent.OrderByDescending(a => a.TimeStamp).ToList();
                });


                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }


        [Consumes("application/json")]
        [HttpPost("/WebApi/MessageModule/SetDiscussionContribution")]
        public async Task<IActionResult> SetDiscussionContribution([FromBody] WebDiscussionContribution contribution) {
            try {
                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;

                SolutionMessageModuleList parentMessage;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    parentMessage = new hotelsContext().SolutionMessageModuleLists.Where(a => a.Id == contribution.ParentId && a.MessageType.Name == "discussionForum").FirstOrDefault();
                }
                if (parentMessage != null) {
                    SolutionMessageModuleList answerMessage = new() {
                        Level = parentMessage.Level + 1, MessageParentId = contribution.ParentId, MessageTypeId = parentMessage.MessageTypeId,
                        Subject = contribution.Subject, HtmlMessage = "<html>\r\n<head>\r\n<meta content=\"text/html;utf-8\" http-equiv=\"content-type\">\r\n</head>\r\n<body>" + contribution.Message + "</body>\r\n</html>",
                        IsSystemMessage = false, Published = true, Shown = false, Archived = false,
                        GuestId = int.Parse(authId), UserId = parentMessage.UserId
                    };

                    var data = new hotelsContext().SolutionMessageModuleLists.Add(answerMessage);
                    int result = await data.Context.SaveChangesAsync();

                    //TODO send emails to ALL recipients

                    return Ok(JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.success.ToString(), ErrorMessage = string.Empty }));
                }

            } catch (Exception ex) { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
            return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DbOperations.DBTranslate("DiscussionContributionIsNotValid", contribution.Language) });
        }



        #endregion DiscussionForum

    }
}