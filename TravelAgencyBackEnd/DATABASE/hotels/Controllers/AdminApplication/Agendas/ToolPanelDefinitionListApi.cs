
using UbytkacBackend;

namespace Golden.Controllers {

    [Authorize]
    [ApiController]
    [Route("ToolPanelDefinitionList")]
    public class ToolPanelDefinitionListApi : ControllerBase {

        [HttpGet("/ToolPanelDefinitionList")]
        public async Task<string> GetToolPanelDefinitionList() {
            List<ToolPanelDefinitionList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().ToolPanelDefinitionLists.ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/ToolPanelDefinitionList/Filter/{filter}")]
        public async Task<string> GetToolPanelDefinitionListByFilter(string filter) {
            List<ToolPanelDefinitionList> data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
            })) {
                data = new hotelsContext().ToolPanelDefinitionLists.FromSqlRaw("SELECT * FROM ToolPanelDefinitionList WHERE 1=1 AND " + filter.Replace("+", " ")).AsNoTracking().ToList();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpGet("/ToolPanelDefinitionList/{id}")]
        public async Task<string> GetToolPanelDefinitionListKey(int id) {
            ToolPanelDefinitionList data;
            using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                IsolationLevel = IsolationLevel.ReadUncommitted
            })) {
                data = new hotelsContext().ToolPanelDefinitionLists.Where(a => a.Id == id).First();
            }

            return JsonSerializer.Serialize(data);
        }

        [HttpPut("/ToolPanelDefinitionList")]
        [Consumes("application/json")]
        public async Task<string> InsertToolPanelDefinitionList([FromBody] ToolPanelDefinitionList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    var data = new hotelsContext().ToolPanelDefinitionLists.Add(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = ServerCoreDbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) });
            }
        }

        [HttpPost("/ToolPanelDefinitionList")]
        [Consumes("application/json")]
        public async Task<string> UpdateToolPanelDefinitionList([FromBody] ToolPanelDefinitionList record) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    var data = new hotelsContext().ToolPanelDefinitionLists.Update(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = ServerCoreDbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpDelete("/ToolPanelDefinitionList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteToolPanelDefinitionList(string id) {
            try {
                if (Request.HttpContext.User.IsInRole("Admin".ToLower())) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    ToolPanelDefinitionList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().ToolPanelDefinitionLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.DeniedYouAreNotAdmin.ToString(), RecordCount = 0, ErrorMessage = ServerCoreDbOperations.DBTranslate(DBResult.DeniedYouAreNotAdmin.ToString()) });
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) });
            }
        }
    }
}