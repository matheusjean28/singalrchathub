using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        public AuthService(IConfiguration configuration, ILogger<AuthService> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GenerateJwtToken(string UserName) 
        {
            try
            {
            _logger.LogInformation("UserName inside authservice is this Here Is :" + UserName);
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
              var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
       _logger.LogInformation("Generated JWT token: " + jwtToken);
            return await Task.FromResult(jwtToken); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
