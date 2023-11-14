namespace UbytkacBackend.Controllers {

    /// <summary>
    /// The web login api.
    /// </summary>
    [ApiController]
    [Route("WebApi/Guest")]
    public class WebLoginApi : ControllerBase {
        private static Encoding ISO_8859_1_ENCODING = Encoding.GetEncoding("ISO-8859-1");

        /// <summary>
        /// The web login.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <param name="language">     The language.</param>
        /// <returns>The result.</returns>
        [AllowAnonymous]
        [HttpPost("/WebApi/Guest/WebLogin")]
        [Consumes("application/json")]
        public IActionResult WebLogin([FromHeader] string authorization, [FromBody] PageLanguage language) {
            (string email, string password) = GetUsernameAndPasswordFromAuthorizeHeader(authorization);

            GuestLoginResponse guest = GuestLogin(email, password);

            try
            {
                if (HttpContext.Connection.RemoteIpAddress != null && guest != null)
                {
                    string clientIPAddr = System.Net.Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                    if (!string.IsNullOrWhiteSpace(clientIPAddr)) { ServerCoreDbOperations.WriteWebLogin(clientIPAddr, guest.Id, email); }
                }
            }
            catch { }

            if (guest == null)
                return BadRequest(new { message = ServerCoreDbOperations.DBTranslate("IncorrectEmailOrPassword", language.Language) });

            RefreshGuestToken(email, guest);
            return Ok(JsonSerializer.Serialize(guest));
        }

        private static (string?, string?) GetUsernameAndPasswordFromAuthorizeHeader(string authorizeHeader) {
            if (authorizeHeader == null || (!authorizeHeader.Contains("Basic ") && !authorizeHeader.Contains("Bearer "))) return (null, null);

            if (authorizeHeader.Contains("Basic "))
            {
                string encodedUsernamePassword = authorizeHeader.Substring("Basic ".Length).Trim();
                string emailPassword = ISO_8859_1_ENCODING.GetString(Convert.FromBase64String(encodedUsernamePassword));

                string email = emailPassword.Split(':')[0];
                string password = emailPassword.Split(':')[1];

                return (email, password);
            }

            return (null, null);
        }

        /// <summary>
        /// The guest login.
        /// </summary>
        /// <param name="email">   The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>The result.</returns>
        public static GuestLoginResponse? GuestLogin(string? email, string? password) {
            if (string.IsNullOrWhiteSpace(email)) return null;

            var guest = new hotelsContext().GuestLists.Where(a => a.Active == true && a.Email.ToLower() == email.ToLower()).FirstOrDefault();

            if (guest != null) guest = BCrypt.Net.BCrypt.Verify(password, guest.Password) ? guest : null;
            if (guest == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.PrimarySid, guest.Id.ToString()),
                    new Claim(ClaimTypes.Email, guest.Email),
                    new Claim(ClaimTypes.NameIdentifier, guest.Email.ToString()),
                    new Claim(ClaimTypes.GroupSid, guest.UserId.ToString()),
                }),
                Issuer = guest.Email,
                //NotBefore = DateTimeOffset.Now.DateTime,
                //Expires = DateTimeOffset.Now.AddMinutes(0).DateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            GuestLoginResponse authResponse = new()
            {
                Id = guest.Id,
                FirstName = guest.FirstName,
                LastName = guest.LastName,
                Street = guest.Street,
                ZipCode = guest.ZipCode,
                City = guest.City,
                Country = guest.Country,
                Phone = guest.Phone,
                Email = guest.Email,
                UserId = guest.UserId.ToString(),
                Token = tokenHandler.WriteToken(token)
            };
            return authResponse;
        }

        /// <summary>
        /// The refresh guest token.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="token">   The token.</param>
        /// <returns>The result.</returns>
        public static bool RefreshGuestToken(string username, GuestLoginResponse token) {
            var dbUser = new hotelsContext()
                .GuestLists.Where(a => a.Active == true && a.Email == username)//.Include(b => b.Role)
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