using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks; 

namespace AuthServiceJwt
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateJwtToken(string UserName) 
        {
            try
            {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:secreteKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, UserName),
            };

            var token = new JwtSecurityToken(
                _configuration["jwt:issuer"],
                _configuration["jwt:audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token)); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
