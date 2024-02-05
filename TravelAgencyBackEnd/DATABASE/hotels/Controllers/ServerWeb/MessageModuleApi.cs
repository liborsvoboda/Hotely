using UbytkacBackend.DBModel;
using UbytkacBackend;

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
                        && ((a.MesssageParentId != null && a.MesssageParent.MesssageParentId != a.Id) || a.MesssageParentId == null)
                        && a.Published && ((!archived && !a.Archived) || archived))
                        .Include(a => a.MesssageParent).ThenInclude(a => a.MesssageParent).ThenInclude(a => a.MesssageParent).ThenInclude(a => a.MesssageParent)
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
                    data = new hotelsContext().MessageModuleLists.Where(a => a.MessageType.Name == "reservation" && a.Published && ((!archived && !a.Archived) || archived)).OrderByDescending(a => a.TimeStamp).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }



        #endregion Web Messages Controls

    }
}