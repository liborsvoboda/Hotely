namespace UbytkacBackend.Controllers {

    [ApiController]
    [Route("WebApi/WebPages")]
     //[ApiExplorerSettings(IgnoreApi = true)]
    public class WebPagesAdminToolsApi : ControllerBase {

        [HttpGet("/WebApi/WebPages/WebAdmin/WebToolList/{webOnly}")]
        public async Task<string> GetWebToolList(bool webOnly = false) {
            try {
                //if (CommunicationController.IsAdmin()) {
                List<ServerToolPanelDefinitionList> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                })) {
                    if (webOnly) {
                        data = new hotelsContext().ServerToolPanelDefinitionLists.Include(a => a.ToolType).Where(a => a.ToolType.Id == 11 || a.ToolType.Id == 1006 || a.ToolType.Id == 1009).OrderBy(a => a.ToolType.Sequence).ToList();
                    }
                    else { data = new hotelsContext().ServerToolPanelDefinitionLists.Include(a => a.ToolType).OrderBy(a => a.ToolType.Sequence).ToList(); }
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                //}
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetSystemErrMessage(ex) }); }
        }

        [HttpGet("/WebApi/WebPages/WebAdmin/WebCodeLibraryList")]
        public async Task<string> GetWebCodeLibrary() {
            try {
                //if (CommunicationController.IsAdmin()) {
                List<WebCodeLibraryList> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                })) {
                    data = new hotelsContext().WebCodeLibraryLists.OrderBy(a => a.Name).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                //}
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetSystemErrMessage(ex) }); }
        }

        [HttpGet("/WebApi/WebPages/WebAdmin/GetWebMenuList")]
        public async Task<string> GetTemplateWebMenuList() {
            try {
                //if (CommunicationController.IsAdmin()) {
                List<WebMenuList> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                })) {
                    data = new hotelsContext().WebMenuLists
                        .Include(a => a.Group).OrderBy(a => a.Name).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetSystemErrMessage(ex) }); }
        }

        [Authorize]
        [HttpPost("/WebApi/WebPages/WebAdmin/SetMenuList")]
        [Consumes("application/json")]
        public async Task<string> InsertOrUpdateWebMenuList([FromBody] WebSettingList1 record) {
            try {
                if (CommunicationController.IsAdmin()) {
                    string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                    string clientIPAddr = null;
                    int RecId = int.Parse(record.Settings.FirstOrDefault(a => a.Key == "Id").Value);
                    if (HttpContext.Connection.RemoteIpAddress != null) { clientIPAddr = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString(); }

                    WebMenuList webMenu = new WebMenuList() {
                        Id = RecId,
                        GroupId = int.Parse(record.Settings.FirstOrDefault(a => a.Key == "GroupId").Value),
                        Sequence = int.Parse(record.Settings.FirstOrDefault(a => a.Key == "Sequence").Value),
                        Name = record.Settings.FirstOrDefault(a => a.Key == "Name").Value,
                        MenuClass = record.Settings.FirstOrDefault(a => a.Key == "MenuClass").Value,
                        Description = record.Settings.FirstOrDefault(a => a.Key == "Description").Value,
                        HtmlContent = record.Settings.FirstOrDefault(a => a.Key == "HtmlContent").Value,
                        UserIpaddress = clientIPAddr,
                        UserId = int.Parse(authId),
                        UserMenu = bool.Parse(record.Settings.FirstOrDefault(a => a.Key == "UserMenu").Value),
                        AdminMenu = bool.Parse(record.Settings.FirstOrDefault(a => a.Key == "AdminMenu").Value),
                        Active = bool.Parse(record.Settings.FirstOrDefault(a => a.Key == "Active").Value),
                        TimeStamp = DateTimeOffset.Now.DateTime
                    };

                    int result = 0;
                    if (RecId == 0) {
                        var data = new hotelsContext().WebMenuLists.Add(webMenu);
                        result = await data.Context.SaveChangesAsync();
                    }
                    else {
                        var data = new hotelsContext().WebMenuLists.Update(webMenu);
                        result = await data.Context.SaveChangesAsync();
                    }

                    if (result > 0) return JsonSerializer.Serialize(new DBResultMessage() { InsertedId = webMenu.Id, Status = DBResult.success.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                    else return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = result, ErrorMessage = string.Empty });
                }
                else {
                    return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") });
                }
            } catch (Exception ex) {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) });
            }
        }

        [Authorize]
        [HttpDelete("/WebApi/WebPages/WebAdmin/DeleteWebMenuList/{id}")]
        [Consumes("application/json")]
        public async Task<string> DeleteWebMenuList(string id) {
            try {
                if (CommunicationController.IsAdmin()) {
                    if (!int.TryParse(id, out int Ids)) return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = "Id is not set" });

                    WebMenuList record = new() { Id = int.Parse(id) };

                    var data = new hotelsContext().WebMenuLists.Remove(record);
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