namespace UbytkacBackend.Controllers {

    /// <summary>
    /// StoredSpocedures Central Library With Return Dynamic DataList
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase"/>
    [Authorize]
    [ApiController]
    [Route("StoredProceduresList")]
    public class UbytkacBackendStoredProceduresListApi : ControllerBase {




        [HttpPost("/ServerApi/DatabaseServices/SpProcedure/GetGenericDataListbyParams")]
        [Consumes("application/json")]
        public async Task<string> GetSystemOperationsList(List<Dictionary<string, string>> dataset) {
            string procedureName = ""; string parameters = ""; string EntityTypeName = "";
            foreach (Dictionary<string, string> param in dataset) {
                if (param.Where(a => a.Key.ToLower() == "SpProcedure".ToLower()).Any()) {
                    procedureName = param.Where(a => a.Key.ToLower() == "SpProcedure".ToLower()).First().Value;
                }
                else if (param.Where(a => a.Key.ToLower() == "tableName".ToLower()).Any()) {
                    parameters += (parameters.Length > 0 ? "," : "") + $"@{param.Keys.First()} = N'{param.Values.First()}' ";
                    EntityTypeName = param.Values.First();
                }
                else { parameters += (parameters.Length > 0 ? "," : "") + $"@{param.Keys.First()} = N'{param.Values.First()}' "; }
            }
            var data = DataOperations.CreateObjectTypeByTypeName(EntityTypeName);
            data = new hotelsContext().ExecuteReader($"EXEC {procedureName} {parameters};");
            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true, DictionaryKeyPolicy = JsonNamingPolicy.CamelCase, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }



        [HttpGet("/StoredProceduresList/Message/{procedureName}")]
        public async Task<string> GetSystemOperationsList(string procedureName) {
            List<SystemOperationMessage> data = new List<SystemOperationMessage>();
            data = new hotelsContext().EasyITCenterCollectionFromSql<SystemOperationMessage>($"EXEC {procedureName};");

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }


        [HttpGet("/StoredProceduresList/Json/{procedureName}")]
        public async Task<string> GetSystemOperationsListJson(string procedureName) {
            List<DBJsonFile> data = null;
            data = new hotelsContext().EasyITCenterCollectionFromSql<DBJsonFile>($"EXEC {procedureName};");
            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }

        /// <summary>
        /// Gets Table List for Reporting
        /// </summary>
        /// <returns></returns>
        [HttpGet("/StoredProceduresList/SpGetTableList")]
        public async Task<string> GetSpGetTableList() {
            try {
                List<CustomString> data = new();
                data = new hotelsContext().EasyITCenterCollectionFromSql<CustomString>("EXEC SpGetTableList;");
                return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        /// <summary>
        /// Gets Form Agendas Pages List For System Menu Definition.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/StoredProceduresList/SpGetSystemPageList")]
        public async Task<string> GetSpGetSystemPageList() {
            try {
                List<CustomString> data = new();
                data = new hotelsContext().EasyITCenterCollectionFromSql<CustomString>("EXEC SpGetSystemPageList;");
                return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        /// <summary>
        /// Api For Logged User with Menu Datalist
        /// </summary>
        /// <returns></returns>
        [HttpGet("/StoredProceduresList/SpGetUserMenuList")]
        public async Task<string> GetSpGetUserMenuList() {
            try {
                List<SpUserMenuList> data = new List<SpUserMenuList>();

                data = new hotelsContext().EasyITCenterCollectionFromSql<SpUserMenuList>("EXEC SpGetUserMenuList @userRole = N'" + User.FindFirst(ClaimTypes.Role.ToString())?.Value + "';");
                return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}