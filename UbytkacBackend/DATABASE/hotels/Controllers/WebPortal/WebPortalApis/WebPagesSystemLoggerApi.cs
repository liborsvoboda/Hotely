namespace UbytkacBackend.Controllers {

    [AllowAnonymous]
    [ApiController]
    [Route("WebApi/WebPages")]
     //[ApiExplorerSettings(IgnoreApi = true)]
    public class WebPagesSystemLoggerApi : ControllerBase {

        [HttpPost("/WebApi/WebPages/WebSystem/SetWebSystemLogMessage")]
        public async Task<string> SetWebLogMessage([FromBody] WebSystemLogMessage record) {
            try {
                SolutionFailList solutionFailList = new SolutionFailList() {
                    Source = "WebPortal",
                    LogLevel = record.LogLevel,
                    Message = record.Message,
                    UserId = record.UserId,
                    UserName = record.UserName,
                    TimeStamp = DateTimeOffset.Now.DateTime,
                    AttachmentName = record.AttachmentName,
                    Attachment = record.Attachment,
                    ImageName = record.ImageName,
                    Image = record.Image
                };
                var data = new hotelsContext().SolutionFailLists.Add(solutionFailList);
                int result = await data.Context.SaveChangesAsync();

                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.success.ToString(), ErrorMessage = string.Empty });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}