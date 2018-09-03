using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.Models;

namespace TaskPlanner.CSFiles
{
    public class JWTTokenGenerator
    {
        public string Token => this.GenerateToken();

        private IConfiguration _config;
        private ApplicationUser _user;

        public JWTTokenGenerator(IConfiguration config, ApplicationUser user)
        {
            _config = config;
            _user = user;
        }


        private string GenerateToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Auth:Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claim = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, this._user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,_user.Id),
                new Claim(JwtRegisteredClaimNames.FamilyName,_user.LastName),
                new Claim(JwtRegisteredClaimNames.GivenName,_user.FirstName)
            };
            var token = new JwtSecurityToken(_config["Auth:Jwt:Issuer"],
               _config["Auth:Jwt:Audience"],
               expires: DateTime.Now.AddMinutes(30),
               claims: claim,
               signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
