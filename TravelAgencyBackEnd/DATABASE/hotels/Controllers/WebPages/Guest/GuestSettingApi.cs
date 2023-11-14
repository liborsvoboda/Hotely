namespace UbytkacBackend.Controllers {

    [Authorize]
    [ApiController]
    [Route("WebApi/Guest")]
    public class GuestSettingApi : ControllerBase {

        [HttpGet("/WebApi/Guest/GetGuestSettingList/{guestId}")]
        [Consumes("application/json")]
        public async Task<string> GetGuestSettingListByGuestId(int guestId) {
            List<GuestSettingList> data;
            try {
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    data = new hotelsContext().GuestSettingLists.Where(a => a.GuestId == guestId).ToList();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            } catch (Exception ex) { return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = ServerCoreFunctions.GetUserApiErrMessage(ex) }); }
        }

        [HttpPost("/WebApi/Guest/SetGuestSettingList")]
        [Consumes("application/json")]
        public IActionResult SetGuestSettingList([FromBody] WebGuestSettingList record) {
            try {

                string authId = User.FindFirst(ClaimTypes.PrimarySid.ToString()).Value;
                List<GuestSettingList> guestSettingList;
                guestSettingList = new hotelsContext().GuestSettingLists.Where(a => a.GuestId == int.Parse(authId)).ToList();

                record.Settings.ForEach(setting => {

                    if (guestSettingList.FirstOrDefault(a => a.Key == setting.Key) == null) {
                        EntityEntry<GuestSettingList> data = new hotelsContext().GuestSettingLists.Add(new GuestSettingList() { Key = setting.Key , Value = setting.Value , GuestId = int.Parse(authId)});
                        int result = data.Context.SaveChanges();
                    } else {
                        guestSettingList.FirstOrDefault(a => a.Key == setting.Key).Value = setting.Value;
                        var data = new hotelsContext().GuestSettingLists.Update(guestSettingList.First(a => a.Key == setting.Key));
                        int result = data.Context.SaveChanges();
                    }
                });

                return Ok(JsonSerializer.Serialize(
                new DBResultMessage() {
                    Status = DBResult.success.ToString(),
                    ErrorMessage = string.Empty
                }));
            } catch { }
            return BadRequest(new DBResultMessage() {
                Status = DBResult.error.ToString(),
                ErrorMessage = ServerCoreDbOperations.DBTranslate("SettingsIsNotValid", record.Language)
            });
        }


    }
}