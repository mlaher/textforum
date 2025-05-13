using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using textforum.domain.interfaces;

namespace textforum.logic.services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        IConfiguration _configuration;
        IUserService _userService;

        public UserAuthenticationService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<(bool isValid, string token)> AuthenticateUser(string username, string password)
        {
            var user = await _userService.GetFromCredentials(username, password);

            if (user == null)
                return (false, "");

            var jwtToken = generateJwtToken(new List<Claim>()
            {
                new Claim("userid",user.UserId.ToString()),
                new Claim("email",user.Email),
                new Claim("name",user.Name),
                new Claim("lastname",user.LastName),
                new Claim("createddate",user.CreatedDate.ToString()),
                new Claim("ismoderator",user.IsModerator.ToString())
            });

            return (true, jwtToken);
        }

        public async Task<(bool isValid, IDictionary<string, string?>? claims)> GetClaims(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var result = await tokenHandler.ValidateTokenAsync(jwt, GetValidationParameters());

            if (!result.IsValid)
                return (false, null);

            return (true, result.Claims.ToDictionary(k => k.Key, v => v.Value.ToString()));
        }

        private string generateJwtToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            int expMin = 30;
            int.TryParse(_configuration["JwtExpiryInMinutes"], out expMin);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtIssuer"],
                audience: _configuration["JwtAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expMin),
                signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler();
            var genToken = tokenHandler.WriteToken(token);

            return genToken;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtIssuer"],
                ValidAudience = _configuration["JwtAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"])) // The same key as the one that generate the token
            };
        }
    }
}
