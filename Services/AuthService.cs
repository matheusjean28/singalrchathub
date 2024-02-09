using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserModel;


namespace AuthServiceJwt
{
public class AuthService{

          private readonly IConfiguration _configuration;

            public AuthService(IConfiguration configuration)
        {
        _configuration = configuration;
        }

        public string GenerateJwtToken(User searchedUser)
        {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:secreteKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(ClaimTypes.Name, searchedUser.UserName),
        };

        var token = new JwtSecurityToken(
                _configuration["jwt:issuer"],
                _configuration["jwt:audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
        }
 
}
}