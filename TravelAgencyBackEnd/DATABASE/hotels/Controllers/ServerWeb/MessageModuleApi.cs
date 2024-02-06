using UbytkacBackend.DBModel;
using UbytkacBackend;
using UbytkacBackend.MessageModuleClasses;
using System.ComponentModel.Design;

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
            List<MessageModuleList> data;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().MessageModuleLists.Where(a => a.MessageType.Name == "newsletter" && a.Published && !a.Archived).OrderByDescending(a => a.TimeStamp).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
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
                    data = new hotelsContext().WebSettingLists.Where(a => a.Key == "NewsletterPreviewTemplate").First().Value;
                }
                List<Dictionary<string, string>> htmlData = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(HtmlContent.ToString());
                data = data.Replace("AUTOTITLE", htmlData.Find(a => a.ContainsKey("HtmlTitle")).First().Value);
                data = data.Replace("AUTOCONTENT", htmlData.Find(a => a.ContainsKey("HtmlContent")).First().Value.Split("<BODY>")[1].Split("</BODY>")[0]);

                System.IO.File.WriteAllText(Path.Combine(ServerConfigSettings.StartupPath, "wwwroot", "server-web", "newsletter-preview", "index.html"), data);
                System.IO.File.WriteAllText(Path.Combine(_hostingEnvironment.WebRootPath, "server-web", "newsletter-preview", "index.html"), data);
                return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = 1, ErrorMessage = string.Empty });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
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
            List<MessageModuleList> data;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().MessageModuleLists
                        .Where(a => a.MessageType.Name == "private"
                        && ((a.MessageParentId != null && a.MessageParent.MessageParentId != a.Id) || a.MessageParentId == null)
                        && a.Published && ((!archived && !a.Archived) || archived))
                        .Include(a => a.MessageParent).ThenInclude(a => a.MessageParent).ThenInclude(a => a.MessageParent).ThenInclude(a => a.MessageParent)
                        .Include(a => a.MessageType)
                        .OrderByDescending(a => a.TimeStamp).ToList();
                }

                data.ForEach(message => { message.MessageType.MessageModuleLists = null; });


                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }


        /// <summary>
        /// WebApi Get Reservation Messages
        /// </summary>
        /// <returns></returns>
        [HttpGet("/WebApi/MessageModule/GetReservationMessageList/{archived}")]
        [Consumes("application/json")]
        public async Task<string> GetReservationMessageList(bool archived) {
            List<MessageModuleList> data;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().MessageModuleLists.Where(a => a.MessageType.Name == "reservation" 
                    && a.Published && ((!archived && !a.Archived) || archived))
                        .OrderByDescending(a => a.TimeStamp).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }


        /// <summary>
        /// Guest Set Private Message Answer
        /// </summary>
        /// <param name="messageAnswer"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpPost("/WebApi/MessageModule/SetPrivateMessageAnswer")]
        public async Task<IActionResult> SetPrivateMessageAnswer([FromBody] PrivateMessageAnswer messageAnswer) {
            try {
                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;

                MessageModuleList parentMessage;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    parentMessage = new hotelsContext().MessageModuleLists.Where(a => a.Id == messageAnswer.ParentId && a.GuestId == int.Parse(authId)).FirstOrDefault();
                }
                if (parentMessage != null) {
                    MessageModuleList answerMessage = new() { 
                        MessageParentId = messageAnswer.ParentId,MessageTypeId = parentMessage.MessageTypeId, 
                        Subject = ServerCoreDbOperations.DBTranslate("AnswerFor", messageAnswer.Language) + ": "+ parentMessage.Subject,HtmlMessage = messageAnswer.Message,
                        IsSystemMessage = false, Published = true, Shown = false, Archived = false,
                        GuestId = int.Parse(authId), UserId = parentMessage.UserId
                    };

                    var data = new hotelsContext().MessageModuleLists.Add(answerMessage);
                    int result = await data.Context.SaveChangesAsync();

                    return Ok(JsonSerializer.Serialize( new DBResultMessage() { Status = DBResult.success.ToString(), ErrorMessage = string.Empty }));
                }

            } catch { }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(), ErrorMessage = ServerCoreDbOperations.DBTranslate("PrivateMessageAnswerIsNotValid", messageAnswer.Language)
            });
        }


        #endregion Web Messages Controls

    }
}