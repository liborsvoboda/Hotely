namespace TravelAgencyBackEnd.Controllers {

    [ApiController]
    [Route("Authentication")]
    public class AuthenticationService : ControllerBase {
        private static System.Text.Encoding ISO_8859_1_ENCODING = System.Text.Encoding.GetEncoding("ISO-8859-1");

        [AllowAnonymous]
        [HttpPost("/Authentication")]
        public IActionResult Authenticate([FromHeader] string Authorization) {
            (string username, string password) = GetUsernameAndPasswordFromAuthorizeHeader(Authorization);

            var user = Authenticate(username, password);

            try
            {
                if (HttpContext.Connection.RemoteIpAddress != null && user != null)
                {
                    string clientIPAddr = System.Net.Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                    if (!string.IsNullOrWhiteSpace(clientIPAddr)) { DBOperations.WriteAppLogin(clientIPAddr, user.Id, username); }
                }
            }
            catch { }

            if (user == null)
                return BadRequest(new { message = DBOperations.DBTranslate("IncorrectEmailOrPassword", "en") });

            refreshUserToken(username, user);
            return Ok(JsonSerializer.Serialize(user));
        }

        private static (string?, string?) GetUsernameAndPasswordFromAuthorizeHeader(string authorizeHeader) {
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

        public static AuthenticateResponse? Authenticate(string? username, string? password) {
            if (username == null)
                return null;

            var user = new hotelsContext()
                .UserLists.Include(a => a.Role).Where(a => a.Active == true && a.UserName == username && a.Password == password)
                .FirstOrDefault();

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //new Claim(ClaimTypes.Name, user.Name),
                    //new Claim(ClaimTypes.Surname, user.Surname),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role.SystemName),
                }),
                Issuer = user.UserName,
                //NotBefore = DateTimeOffset.Now.DateTime,
                //Expires = DateTimeOffset.Now.AddMinutes(0).DateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            AuthenticateResponse authResponse = new() { Id = user.Id, Name = user.Name, Surname = user.SurName, Token = tokenHandler.WriteToken(token), Role = user.Role.SystemName };
            return authResponse;
        }

        public static bool refreshUserToken(string username, AuthenticateResponse token) {
            var dbUser = new hotelsContext()
                .UserLists.Where(a => a.Active == true && a.UserName == username).Include(b => b.Role)
                .FirstOrDefault();

            if (dbUser != null) return true;
            return false;
        }

        //public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        //{
        //    if (refreshUserToken(token.Issuer, new AuthenticateResponse() { Token = ((JwtSecurityToken)token).RawData.ToString() }))
        //        return true;
        //    else return false;
        //}
    }
}