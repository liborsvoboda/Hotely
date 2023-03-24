using BACKENDCORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.Json;

namespace BACKENDCORE.Controllers
{
    [ApiController]
    [Route("Authentication")]
    public class AuthenticationService : ControllerBase
    {
        static System.Text.Encoding ISO_8859_1_ENCODING = System.Text.Encoding.GetEncoding("ISO-8859-1");

        [AllowAnonymous]
        [HttpPost("/Authentication")]
        public IActionResult Authenticate([FromHeader] string Authorization)
        {

            (string username, string password) = GetUsernameAndPasswordFromAuthorizeHeader(Authorization);

            try
            {
                if (HttpContext.Connection.RemoteIpAddress != null)
                {
                    string clientIPAddr = System.Net.Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                    if (!string.IsNullOrWhiteSpace(clientIPAddr)) { LoginHistoryService.WriteVisit(clientIPAddr, username); }
                }
            }
            catch { }

            var user = UserService.Authenticate(username, password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            UserService.refreshUserToken(username, user);
            return Ok(JsonSerializer.Serialize(user));
        }

        private static (string?, string?) GetUsernameAndPasswordFromAuthorizeHeader(string authorizeHeader)
        {
            if (authorizeHeader == null || (!authorizeHeader.Contains("Basic ") && !authorizeHeader.Contains("Bearer "))) return (null, null);

            if (authorizeHeader.Contains("Basic "))
            {
                string encodedUsernamePassword = authorizeHeader.Substring("Basic ".Length).Trim();
                string usernamePassword = ISO_8859_1_ENCODING.GetString(Convert.FromBase64String(encodedUsernamePassword));

                string username = usernamePassword.Split(':')[0];
                string password = usernamePassword.Split(':')[1];

                return (username, password);
            }

            return (null, null);
        }
    }
}
