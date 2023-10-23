using UbytkacBackend.DBModel;
using UbytkacBackend.WebPages;

namespace UbytkacBackend.Controllers {


    [ApiController]
    [Route("WebApi/WebPages")]
    public class WebSettingApi : ControllerBase {

        [HttpGet("/WebApi/WebPages/GetSettingList")]
        [Consumes("application/json")]
        public async Task<string> GetSettingList() {
            List<WebSettingList> data;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    data = new hotelsContext().WebSettingLists.ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex) }); }
        }


        /*
        [Authorize]
        [HttpPost("/WebApi/WebPages/SetSettingList")]
        [Consumes("application/json")]
        public IActionResult SetGuestSettingList([FromBody] WebSettingList1 record) {
            try {
                if (Request.HttpContext.User.IsInRole("admin")) {
                    string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                    List<WebSettingList> webSettingList;
                    webSettingList = new hotelsContext().WebSettingLists.ToList();

                    record.Settings.ForEach(setting => {

                        if (webSettingList.FirstOrDefault(a => a.Key == setting.Key) == null) {
                            var data = new hotelsContext().WebSettingLists.Add(new WebSettingList() { Key = setting.Key, Value = setting.Value, UserId = int.Parse(authId),Timestamp = DateTimeOffset.Now.DateTime });
                            int result = data.Context.SaveChanges();
                        }
                        else {
                            webSettingList.FirstOrDefault(a => a.Key == setting.Key).Value = setting.Value;
                            webSettingList.FirstOrDefault(a => a.Key == setting.Key).Timestamp = DateTimeOffset.Now.DateTime;
                            var data = new hotelsContext().WebSettingLists.Update(webSettingList.First(a => a.Key == setting.Key));
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
                    ErrorMessage = SystemFunctions.GetUserApiErrMessage(ex)
                });
            }
        }
        */

    }
}