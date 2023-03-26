using TravelAgencyBackEnd;
using TravelAgencyBackEnd.Classes;
using TravelAgencyBackEnd.DBModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BACKENDCORE.Services
{
    public static class UserService
    {
        public static AuthenticateResponse? Authenticate(string? username, string? password)
        {
            var user = new hotelsContext()
                .UserLists.Include(a => a.Role).Where(a => a.Active == true && a.UserName == username && a.Password == password)
                .First();

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
                NotBefore = DateTimeOffset.Now.DateTime,
                Expires = DateTimeOffset.Now.AddMinutes(0).DateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            AuthenticateResponse authResponse = new() {Id = user.Id, Name = user.Name, Surname = user.SurName, Token = tokenHandler.WriteToken(token) };
            return authResponse;
        }

        public static bool refreshUserToken(string username, AuthenticateResponse token)
        {
            var dbUser = new hotelsContext()
                .UserLists.Where(a => a.Active == true && a.UserName == username)
                .First();

            if (dbUser != null) return true;
            return false;
        }

        public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (refreshUserToken(token.Issuer, new AuthenticateResponse() { Token = ((JwtSecurityToken)token).RawData.ToString() }))
                return true;
            else return false;
        }
    }
}
