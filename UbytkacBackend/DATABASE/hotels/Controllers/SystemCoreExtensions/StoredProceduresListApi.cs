namespace UbytkacBackend.Controllers {

    /// <summary>
    /// StoredSpocedures Central Library With Return Dynamic DataList
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase"/>
    [Authorize]
    [ApiController]
    [Route("StoredProceduresList")]
    public class UbytkacBackendStoredProceduresListApi : ControllerBase {

        /// <summary>
        /// Gets Table List for Reporting
        /// </summary>
        /// <returns></returns>
        [HttpGet("/StoredProceduresList/SystemSpGetTableList")]
        public async Task<string> GetSystemSpGetTableList() {
            try {
                List<CustomString> data = new();
                data = new hotelsContext().UbytkacBackendCollectionFromSql<CustomString>("EXEC SystemSpGetTableList;");
                return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        /// <summary>
        /// Gets Form Agendas Pages List For System Menu Definition.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/StoredProceduresList/SystemSpGetSystemPageList")]
        public async Task<string> GetSystemTableList() {
            try {
                List<CustomString> data = new();
                data = new hotelsContext().UbytkacBackendCollectionFromSql<CustomString>("EXEC SystemSpGetSystemPageList;");
                return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        /// <summary>
        /// Api For Logged User with Menu Datalist
        /// </summary>
        /// <returns></returns>
        [HttpGet("/StoredProceduresList/SystemSpGetUserMenuList")]
        public async Task<string> GetSystemSpGetUserMenuList() {
            try {
                List<SpUserMenuList> data = new List<SpUserMenuList>();

                data = new hotelsContext().UbytkacBackendCollectionFromSql<SpUserMenuList>("EXEC SystemSpGetUserMenuList @userRole = N'" + User.FindFirst(ClaimTypes.Role.ToString())?.Value + "';");
                return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}