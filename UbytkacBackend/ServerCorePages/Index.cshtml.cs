using FubarDev.FtpServer.Authorization.Actions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServerCorePages {

    /// <summary>
    /// Default Page for Every Web Request Here are defined Main Pages Sections THIs Page Is Alone For
    /// </summary>
    /// <seealso cref="PageModel"/>
    public class IndexModel : PageModel {
        public static ServerWebPagesToken ServerWebPagesToken;
        public static string? action;

        /// <summary>
        /// Checking Cookie TOKEN FROM Metro for User/UserRole checking on Server Side This Is Use
        /// For User Checking In Razor/MVC Server Web Pages This is Use For User Role and his Rights
        /// If is Logged Checking Has Loaded User Claims with Full Token Info
        /// </summary>
        public void OnGet() {
            string? requestToken = HttpContext.Request.Cookies.FirstOrDefault(a => a.Key == "ApiToken").Value;
            if (!string.IsNullOrWhiteSpace(requestToken)) {
                ServerWebPagesToken = CoreOperations.CheckTokenValidityFromString(requestToken);
                if (ServerWebPagesToken.IsValid) { User.AddIdentities(ServerWebPagesToken.UserClaims.Identities); }
            }
            else { ServerWebPagesToken = new ServerWebPagesToken(); }
        }


        public void SetAction(string? _action) {
            action = _action;
        }

      
    }
}