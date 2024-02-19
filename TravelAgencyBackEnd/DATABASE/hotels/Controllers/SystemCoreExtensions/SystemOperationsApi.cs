namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("UbytkacBackendSystemOperations")]
    public class UbytkacBackendSystemOperationsListApi : ControllerBase {

        [HttpGet("/UbytkacBackendSystemOperations/{procedureName}")]
        public async Task<string> GetSystemOperationsList(string procedureName) {
            List<SystemOperationMessage> data = new List<SystemOperationMessage>();
            data = new hotelsContext().UbytkacBackendCollectionFromSql<SystemOperationMessage>($"EXEC {procedureName};");

            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }


        [HttpGet("/UbytkacBackendSystemOperations/Json/{procedureName}")]
        public async Task<string> GetSystemOperationsListJson(string procedureName) {
            List<DBJsonFile> data = null;
            data = new hotelsContext().UbytkacBackendCollectionFromSql<DBJsonFile>($"EXEC {procedureName};");
            return JsonSerializer.Serialize(data, new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true });
        }
    }
}