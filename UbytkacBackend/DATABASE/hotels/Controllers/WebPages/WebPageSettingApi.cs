
namespace UbytkacBackend.Controllers {


    [ApiController]
    [Route("WebApi/WebPages")]
    public class WebPageSettingApi : ControllerBase {

        [HttpGet("/WebApi/WebPages/GetSettingList")]
        [Consumes("application/json")]
        public async Task<string> GetSettingList() {
            List<WebPageSettingList> data;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    data = new hotelsContext().WebPageSettingLists.ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } 
            catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DataOperations.GetUserApiErrMessage(ex) }); }
        }


        /*
        [Authorize]
        [HttpPost("/WebApi/WebPages/SetSettingList")]
        [Consumes("application/json")]
        public IActionResult SetGuestSettingList([FromBody] WebPageSettingList1 record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin".ToLower())) {
                    string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                    List<WebPageSettingList> WebPageSettingList;
                    WebPageSettingList = new hotelsContext().WebPageSettingLists.ToList();

                    record.Settings.ForEach(setting => {

                        if (WebPageSettingList.FirstOrDefault(a => a.Key == setting.Key) == null) {
                            var data = new hotelsContext().WebPageSettingLists.Add(new WebPageSettingList() { Key = setting.Key, Value = setting.Value, UserId = int.Parse(authId),Timestamp = DateTimeOffset.Now.DateTime });
                            int result = data.Context.SaveChanges();
                        }
                        else {
                            WebPageSettingList.FirstOrDefault(a => a.Key == setting.Key).Value = setting.Value;
                            WebPageSettingList.FirstOrDefault(a => a.Key == setting.Key).Timestamp = DateTimeOffset.Now.DateTime;
                            var data = new hotelsContext().WebPageSettingLists.Update(WebPageSettingList.First(a => a.Key == setting.Key));
                            int result = data.Context.SaveChanges();
                        }
                    });

                    return Ok(JsonSerializer.Serialize(
                    new DBResultMessage() {
                        Status = DBResult.success.ToString(),
                        ErrorMessage = string.Empty
                    }));

                } else {
                    return BadRequest(new DBResultMessage() {
                        Status = DBResult.error.ToString(),
                        ErrorMessage = DBOperations.DBTranslate("YouDoesNotHaveRights")
                    });
                }
            } catch (Exception ex) {
                return BadRequest(new DBResultMessage() {
                    Status = DBResult.error.ToString(),
                    ErrorMessage = SystemDataOperations.GetUserApiErrMessage(ex)
                });
            }
        }
        */

    }
}