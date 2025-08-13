namespace UbytkacBackend.Controllers {

    [AllowAnonymous]
    [ApiController]
    [Route("WebApi/WebPages")]
     //[ApiExplorerSettings(IgnoreApi = true)]
    public class WebPagesMenuListApi : ControllerBase {

        [Authorize]
        [HttpGet("/WebApi/WebPages/GetAdminWebMenuList")]
        public async Task<string> GetAdminWebMenuList() {
            if (ServerApiServiceExtension.IsAdmin()) {
                bool IpIsBlocked = false;
                // check If blocked and write Visit On menu load
                try {
                    if (HttpContext.Connection.RemoteIpAddress != null) {
                        string clientIPAddr = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                        if (!string.IsNullOrWhiteSpace(clientIPAddr)) { SoftwareTriggers.WriteWebVisit(clientIPAddr); }
                        using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                            IpIsBlocked = new hotelsContext().WebBannedIpAddressLists.Where(a => a.IpAddress == clientIPAddr && a.Active).Count() > 0;
                        }
                    }
                } catch { }

                if (!IpIsBlocked) {
                    List<WebMenuList> data;
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                        IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                    })) {
                        data = new hotelsContext().WebMenuLists
                            .Include(a => a.Group)
                            .OrderBy(a => a.Sequence)
                            .ToList().OrderBy(a => a.Group.Sequence).ToList();
                    }

                    data.ForEach(menu => { menu.HtmlContent = null; });
                    return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        WriteIndented = true,
                        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                }
                else {
                    return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("IpAddressIsBlocked") });
                }
            }
            else {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("YouDoesNotHaveRights") });
            }
        }

        [HttpGet("/WebApi/WebPages/GetWebMenuList")]
        public async Task<string> GetWebMenuList() {
            bool IpIsBlocked = false;
            // check If blocked and write Visit On menu load
            try {
                if (HttpContext.Connection.RemoteIpAddress != null) {
                    string clientIPAddr = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                    if (!string.IsNullOrWhiteSpace(clientIPAddr)) { SoftwareTriggers.WriteWebVisit(clientIPAddr); }
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        IpIsBlocked = new hotelsContext().WebBannedIpAddressLists.Where(a => a.IpAddress == clientIPAddr && a.Active).Count() > 0;
                    }
                }
            } catch { }

            if (!IpIsBlocked) {
                List<WebMenuList> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    data = new hotelsContext().WebMenuLists
                        .Include(a => a.Group)
                        .Where(a => !a.AdminMenu && !a.UserMenu && a.Active && a.Group.Active)
                        .OrderBy(a => a.Sequence)
                        .ToList().OrderBy(a => a.Group.Sequence).ToList();
                }

                data.ForEach(menu => { menu.HtmlContent = null; });
                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            else {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("IpAddressIsBlocked") });
            }
        }

        [Authorize]
        [HttpGet("/WebApi/WebPages/GetAuthWebMenuList")]
        public async Task<string> GetAuthWebMenuList() {
            bool IpIsBlocked = false;
            string authId = null;
            string authRole = null;
            // check If blocked and write Visit On menu load
            try {
                if (HttpContext.Connection.RemoteIpAddress != null) {
                    string clientIPAddr = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                    if (!string.IsNullOrWhiteSpace(clientIPAddr)) { SoftwareTriggers.WriteWebVisit(clientIPAddr); }
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        IpIsBlocked = new hotelsContext().WebBannedIpAddressLists.Where(a => a.IpAddress == clientIPAddr && a.Active).Count() > 0;
                    }
                }
            } catch { }

            authId = User.FindFirstValue(ClaimTypes.PrimarySid.ToString());
            authRole = User.FindFirstValue(ClaimTypes.Role.ToString()).ToLower();

            if (!IpIsBlocked) {
                List<WebMenuList> data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions {
                    IsolationLevel = IsolationLevel.ReadUncommitted //with NO LOCK
                })) {
                    data = new hotelsContext().WebMenuLists
                        .Include(a => a.Group)
                        .Where(a => ((!a.AdminMenu && !a.UserMenu) || ((a.UserMenu || !a.AdminMenu) && authRole == "webuser") || ((a.UserMenu || a.AdminMenu) && authRole == "admin")) && a.Active && a.Group.Active)
                        .OrderBy(a => a.Sequence)
                        .ToList().OrderBy(a => a.Group.Sequence).ToList();
                }

                data.ForEach(menu => { menu.HtmlContent = null; });
                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            else {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("IpAddressIsBlocked") });
            }
        }

        [HttpGet("/WebApi/WebPages/GetWebMenuListById/{id}")]
        public async Task<string> GetWebMenuListById(int id) {
            bool IpIsBlocked = false;

            // check If blocked and write Visit On menu load
            try {
                if (HttpContext.Connection.RemoteIpAddress != null) {
                    string clientIPAddr = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                    if (!string.IsNullOrWhiteSpace(clientIPAddr)) { SoftwareTriggers.WriteWebVisit(clientIPAddr); }
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        IpIsBlocked = new hotelsContext().WebBannedIpAddressLists.Where(a => a.IpAddress == clientIPAddr && a.Active).Count() > 0;
                    }
                }
            } catch { }

            if (!IpIsBlocked) {
                WebMenuList data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    //string authRole = User.FindFirstValue(ClaimTypes.Role.ToString()).ToLower();

                    data = new hotelsContext().WebMenuLists.Where(a => a.Id == id).FirstOrDefault();

                    if (data == null) data = new hotelsContext().WebMenuLists
                            .Where(a => !a.AdminMenu && !a.UserMenu && a.Active && a.Group.Active && a.Default).FirstOrDefault();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            else {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("IpAddressIsBlocked") });
            }
        }

        [Authorize]
        [HttpGet("/WebApi/WebPages/GetAuthWebMenuListById/{id}")]
        public async Task<string> GetAuthWebMenuListById(int id) {
            bool IpIsBlocked = false;
            string authId = null;
            string authRole = null;
            // check If blocked and write Visit On menu load
            try {
                if (HttpContext.Connection.RemoteIpAddress != null) {
                    string clientIPAddr = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                    if (!string.IsNullOrWhiteSpace(clientIPAddr)) { SoftwareTriggers.WriteWebVisit(clientIPAddr); }
                    using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                        IpIsBlocked = new hotelsContext().WebBannedIpAddressLists.Where(a => a.IpAddress == clientIPAddr && a.Active).Count() > 0;
                    }
                }
            } catch { }

            authId = User.FindFirstValue(ClaimTypes.PrimarySid.ToString());
            authRole = User.FindFirstValue(ClaimTypes.Role.ToString()).ToLower();
            if (!IpIsBlocked) {
                WebMenuList data;
                using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted })) {
                    data = new hotelsContext().WebMenuLists.Where(a => a.Id == id).FirstOrDefault();

                    if (data == null) data = new hotelsContext().WebMenuLists
                            .Where(a => ((!a.AdminMenu && !a.UserMenu) || ((a.UserMenu || !a.AdminMenu) && authRole == "webuser") || ((a.UserMenu || a.AdminMenu) && authRole == "admin")) && a.Active && a.Group.Active && a.Default)
                            .FirstOrDefault();
                }

                return JsonSerializer.Serialize(data, new JsonSerializerOptions() {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            else {
                return JsonSerializer.Serialize(new DBResultMessage() { Status = DBResult.error.ToString(), RecordCount = 0, ErrorMessage = DbOperations.DBTranslate("IpAddressIsBlocked") });
            }
        }
    }
}