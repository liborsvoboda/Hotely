

namespace UbytkacBackend.ServerCoreDBSettings {

    /// <summary>
    /// Server Restart Api for Remote Control
    /// </summary>
    /// <seealso cref="ControllerBase"/>
    [Authorize]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ServerManagementApi : ControllerBase {
        private readonly ILogger logger;

        public ServerManagementApi(ILogger<ServerManagementApi> _logger) => logger = _logger;


        /// <summary>
        /// AutoScheduler Server Controls
        /// </summary>
        /// <returns></returns>
        [HttpGet("/ServerManagement/AutoSchedulerServerStart")]
        public async Task<IActionResult> ServerSchedulerStart() {
            try {
                if (CommunicationController.IsAdmin()) {
                    if (ServerRuntimeData.ServerAutoSchedulerProvider != null) { await ServerRuntimeData.ServerAutoSchedulerProvider.ResumeAll(); }
                    return Ok(JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.success.ToString(), ErrorMessage = string.Empty }));
                }
                else { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") }); }
            } catch (Exception ex) { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpGet("/ServerManagement/AutoSchedulerServerStop")]
        public async Task<IActionResult> ServerSchedulerStop() {
            try {
                if (CommunicationController.IsAdmin()) {
                    if (ServerRuntimeData.ServerAutoSchedulerProvider != null) { await ServerRuntimeData.ServerAutoSchedulerProvider.PauseAll(); }
                    return Ok(JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.success.ToString(), ErrorMessage = string.Empty }));
                }
                else { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") }); }
            } catch (Exception ex) { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpGet("/ServerManagement/AutoSchedulerServerStatus")]
        public async Task<IActionResult> ServerSchedulerStatus() {
            try {
                if (CommunicationController.IsAdmin()) {
                    bool isPaused = false;
                    if (ServerRuntimeData.ServerAutoSchedulerProvider != null) { isPaused = await ServerRuntimeData.ServerAutoSchedulerProvider.IsTriggerGroupPaused("AutoScheduler"); }

                    return Ok(JsonSerializer.Serialize(new DBResultMessage() { Status = isPaused ? ServerStatuses.Stopped.ToString() : ServerStatuses.Running.ToString(), ErrorMessage = string.Empty }));
                }
                else { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") }); }
            } catch (Exception ex) { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }


        /// <summary>
        /// Core Server Restart Control Api
        /// </summary>
        /// <returns></returns>
        [HttpGet("/CoreServerRestart")]
        public async Task<string> ServerRestart() {
            try {
                if (CommunicationController.IsAdmin()) {
                    BackendServer.RestartServer();

                    return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("serverRestarting") });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }


        /// <summary>
        /// FtpServerStart Api
        /// </summary>
        /// <returns></returns>
        [HttpGet("/FtpServerStart")]
        public async Task<string> FtpServerStart() {
            try {
                if (CommunicationController.IsAdmin()) {
                    if (ServerRuntimeData.ServerFTPProvider != null) { await ServerRuntimeData.ServerFTPProvider.StartAsync(); }

                    return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("serverRestarting") });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        /// <summary>
        /// FtpServerStop Api
        /// </summary>
        /// <returns></returns>
        [HttpGet("/FtpServerStop")]
        public async Task<string> FtpServerStop() {
            try {
                if (CommunicationController.IsAdmin()) {
                    if (ServerRuntimeData.ServerFTPProvider != null) { await ServerRuntimeData.ServerFTPProvider.StopAsync(); }

                    return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("serverRestarting") });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }
    }
}