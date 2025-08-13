

namespace UbytkacBackend.ServerCoreDBSettings {

    /// <summary>
    /// Server Restart Api for Remote Control
    /// </summary>
    /// <seealso cref="ControllerBase"/>
    [Authorize]
    [ApiController]
    [Route("ServerApi")]
    public class ServerApiManagementServicesApi : ControllerBase {
        private readonly ILogger logger;

        public ServerApiManagementServicesApi(ILogger<ServerApiManagementServicesApi> _logger) => logger = _logger;


        /// <summary>
        /// AutoScheduler Server Controls
        /// </summary>
        /// <returns></returns>
        [HttpGet("/ServerApi/ManagementServices/AutoSchedulerServerStart")]
        public async Task<IActionResult> ServerSchedulerStart() {
            try {
                if (ServerApiServiceExtension.IsAdmin()) {
                    if (ServerRuntimeData.ServerAutoSchedulerProvider != null) { await ServerRuntimeData.ServerAutoSchedulerProvider.ResumeAll(); }
                    return Ok(JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.success.ToString(), ErrorMessage = string.Empty }));
                }
                else { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") }); }
            } catch (Exception ex) { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpGet("/ServerApi/ManagementServices/AutoSchedulerServerStop")]
        public async Task<IActionResult> ServerSchedulerStop() {
            try {
                if (ServerApiServiceExtension.IsAdmin()) {
                    if (ServerRuntimeData.ServerAutoSchedulerProvider != null) { await ServerRuntimeData.ServerAutoSchedulerProvider.PauseAll(); }
                    return Ok(JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.success.ToString(), ErrorMessage = string.Empty }));
                }
                else { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") }); }
            } catch (Exception ex) { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        [HttpGet("/ServerApi/ManagementServices/AutoSchedulerServerStatus")]
        public async Task<IActionResult> ServerSchedulerStatus() {
            try {
                if (ServerApiServiceExtension.IsAdmin()) {
                    bool isPaused = false;
                    if (ServerRuntimeData.ServerAutoSchedulerProvider != null) { isPaused = await ServerRuntimeData.ServerAutoSchedulerProvider.IsTriggerGroupPaused("AutoScheduler"); }

                    return Ok(JsonSerializer.Serialize(new DBResultMessage() { Status = isPaused ? ServerStatusTypes.Stopped.ToString() : ServerStatusTypes.Running.ToString(), ErrorMessage = string.Empty }));
                }
                else { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") }); }
            } catch (Exception ex) { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }


        /// <summary>
        /// Core Server Restart Control Api
        /// </summary>
        /// <returns></returns>
        [HttpGet("/ServerApi/ManagementServices/CoreServerRestart")]
        public async Task<string> ServerRestart() {
            try {
                if (ServerApiServiceExtension.IsAdmin()) {
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
        [HttpGet("/ServerApi/ManagementServices/FtpServerStart")]
        public async Task<string> FtpServerStart() {
            try {
                if (ServerApiServiceExtension.IsAdmin()) {
                    if (ServerRuntimeData.ServerFTPProvider != null) {
                        ServerRuntimeData.ServerFTPRunningStatus = true;
                        await ServerRuntimeData.ServerFTPProvider.StartAsync();
                    }

                    return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("serverRestarting") });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

        /// <summary>
        /// FtpServerStop Api
        /// </summary>
        /// <returns></returns>
        [HttpGet("/ServerApi/ManagementServices/FtpServerStop")]
        public async Task<string> FtpServerStop() {
            try {
                if (ServerApiServiceExtension.IsAdmin()) {
                    if (ServerRuntimeData.ServerFTPProvider != null) {
                        ServerRuntimeData.ServerFTPRunningStatus = false;
                        await ServerRuntimeData.ServerFTPProvider.StopAsync();
                    }

                    return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = 0, Status = DBResult.success.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("serverRestarting") });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }



        /// <summary>
        /// FTP Server Status
        /// </summary>
        /// <returns></returns>
        [HttpGet("/ServerApi/ManagementServices/FtpServerStatus")]
        public async Task<IActionResult> FtpServerStatus() {
            try {
                if (ServerApiServiceExtension.IsAdmin()) {
                    return Ok(JsonSerializer.Serialize(new DBResultMessage() { Status = !ServerRuntimeData.ServerFTPRunningStatus ? ServerStatusTypes.Stopped.ToString() : ServerStatusTypes.Running.ToString(), ErrorMessage = string.Empty }));
                }
                else { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") }); }
            } catch (Exception ex) { return BadRequest(new DBResultMessage() { Status = DBResult.error.ToString(), ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }

    }
}