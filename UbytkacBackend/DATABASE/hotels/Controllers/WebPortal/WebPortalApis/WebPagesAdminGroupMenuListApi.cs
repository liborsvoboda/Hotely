using UbytkacBackend.DBModel;

namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebApi/WebPages")]
     //[ApiExplorerSettings(IgnoreApi = true)]
    public class WebPagesAdminGroupMenuListApi : ControllerBase {

        [HttpGet("/WebApi/WebPages/WebAdmin/GetWebGroupMenuList")]
        public async Task<string> GetWebGroupMenuList() {
            if (CommunicationController.IsAdmin()) {
                List<WebGroupMenuList> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    data = new hotelsContext().WebGroupMenuLists
                        .OrderBy(a => a.Sequence).ThenBy(a => a.Name)
                        .ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            else { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") }); }
        }

        /// <summary>
        /// WebAdmin API
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        [HttpPost("/WebApi/WebPages/WebAdmin/SetWebGroupMenuList")]
        [Consumes("application/json")]
        public async Task<string> InsertOrUpdateGroupWebMenuList([FromBody] WebSettingList1 record) {
            try {
                if (CommunicationController.IsAdmin()) {
                    string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;

                    int RecId = int.Parse(record.Settings.FirstOrDefault(a => a.Key == "Id").Value);
                    WebGroupMenuList groupWebMenu = new WebGroupMenuList() {
                        Id = RecId,
                        Sequence = int.Parse(record.Settings.FirstOrDefault(a => a.Key == "Sequence").Value),
                        Name = record.Settings.FirstOrDefault(a => a.Key == "Name").Value,
                        Onclick = record.Settings.FirstOrDefault(a => a.Key == "Onclick").Value,
                        Active = bool.Parse(record.Settings.FirstOrDefault(a => a.Key == "Active").Value),
                        UserId = int.Parse(authId),
                        TimeStamp = DateTimeOffset.Now.DateTime
                    };

                    int result = 0;
                    if (RecId == 0) {
                        var data = new hotelsContext().WebGroupMenuLists.Add(groupWebMenu);
                        result = await data.Context.SaveChangesAsync();
                    }
                    else {
                        var data = new hotelsContext().WebGroupMenuLists.Update(groupWebMenu);
                        result = await data.Context.SaveChangesAsync();
                    }

                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = groupWebMenu.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else {
                    return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") });
                }
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        /// <summary>
        /// WebAdmin API
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("/WebApi/WebPages/WebAdmin/DeleteWebGroupMenuList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteWebGroupMenuList(string id) {
            try {
                if (CommunicationController.IsAdmin()) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    WebGroupMenuList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().WebGroupMenuLists.Remove(record);
                    int result = await data.Context.SaveChangesAsync();
                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = record.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else {
                    return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") });
                }
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }
    }
}