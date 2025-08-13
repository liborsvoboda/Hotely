namespace UbytkacBackend.ServerCoreDBSettings {

    /// <summary>
    /// Server Email sender Api for logged Communication
    /// </summary>
    /// <seealso cref="ControllerBase"/>
    [Authorize]
    [ApiController]
    [Route("ServerApi")]
    public class ServerEmailerApi : ControllerBase {

        [HttpGet("/ServerApi/ServerEmailer/{message}")]
        public async Task<string> GetServerEmailer(string message) {
            try {
                string result = null;
                if (!string.IsNullOrWhiteSpace(message)) result = CoreOperations.SendEmail(new SendMailRequest() { Content = message }, true);

                return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(result) });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpPost("/ServerApi/ServerEmailer")]
        [Consumes("application/json")]
        public async Task<string> PostServerEmailer([FromBody] SendMailRequest message) {
            try {
                string? result = null;
                if (!string.IsNullOrWhiteSpace(message.Content)) result = CoreOperations.SendEmail(message, true);

                return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(result) });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpPost("/ServerApi/ServerEmailer/Mass")]
        [Consumes("application/json")]
        public async Task<string> PostServerMassEmailer([FromBody] List<SendMailRequest> messages) {
            try {
                if (ServerConfigSettings.ServiceEnableMassEmail) {
                    CoreOperations.SendMassEmail(messages);
                    return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("emailsSent") });
                }
                else { return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("massEmailNotEnabled") }); }
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }
    }
}