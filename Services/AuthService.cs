using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
                var securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["jwt:secreteKey"])
                );
                var credentials = new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256
                );

                var claims = new[] { new Claim(ClaimTypes.Name, UserName), };

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
                _logger.LogError("Error generating JWT token: " + ex.Message);
                return null;
            }
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["jwt:issuer"],
                    ValidAudience = _configuration["jwt:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["jwt:secreteKey"])
                    ),
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                return tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
