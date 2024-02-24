using System.Diagnostics;
using System.Threading.Tasks;
using AuthServiceJwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserContext;
using UserLoginDTO;
using UserLoginModel;
using UserModel;

namespace AuthControllerMethod
{
    public class AuthController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UserDbContext _context;
        private readonly AuthService _authJwt;

        public AuthController(
            UserDbContext context,
            AuthService authJwt,
            ILogger<UserModel.User> logger
        )
        {
            _context = context;
            _authJwt = authJwt;
            _logger = logger;
        }

        [Route("/CreateUser")]
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserModel.User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Unexpected error occurred");
                }

                if (await CheckIfUserExist(user))
                {
                    var userAlreadyExistMessage = $"{user.UserName} already exists";
                    return Conflict(userAlreadyExistMessage);
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Pass);
                var newUser = new User(user.UserName, user.Email, hashedPassword, user.Gener);
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                var authToken = await _authJwt.GenerateJwtToken(user.UserName);

                var response = new
                {
                    id = newUser.Id,
                    userName = newUser.UserName,
                    email = newUser.Email,
                    token = new { token = authToken }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("An error occurred during account creation.");
            }
        }

        // [Authorize]
        [Route("getAllUser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        //check other params also,
        //return DTO that contains name, userId, gender only
        [Route("Auth")]
        [HttpPost]
        public async Task<IActionResult> Auth([FromBody] UserLogin user)
        {
            try
            {
                var searchedUser = await _context.Users.FirstOrDefaultAsync(u =>
                    u.UserName == user.UserName
                );

                if (searchedUser != null)
                {
                    if (isValidPasswordComparedWithHashed(searchedUser.Pass, user.Password))
                    {
                        var userToken = new
                        {
                            token = await _authJwt.GenerateJwtToken(user.UserName)
                        };

                        var responseAuthUserOk = new UserDTO
                        {
                            Id = searchedUser.Id,
                            UserName = searchedUser.UserName,
                            Email = searchedUser.Email,
                            Token = userToken
                        };

                        return Ok(responseAuthUserOk);
                    }
                    else
                    {
                        return BadRequest("Wrong Password!");
                    }
                }

                var notFoundMessage = $"{user.UserName} was not found.";
                return NotFound(notFoundMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return BadRequest("An error occurred during authentication.");
            }
        }

        //move to other component after finish
        public async Task<bool> CheckIfUserExist(User user)
        {
            var userNameExist = await _context.Users.AnyAsync(u => u.UserName == user.UserName);
            var userEmailExist = await _context.Users.AnyAsync(u => u.Email == user.Email);

            if (userNameExist && userEmailExist)
            {
                return true;
            }
            return false;
        }

        public bool IsValidPassword(string paswordFromParams, string passwordFromDb)
        {
            var isEqualPass = passwordFromDb.Trim().Equals(paswordFromParams.Trim());
            return isEqualPass;
        }

        public bool isValidPasswordComparedWithHashed(string hashedPassword, string plainPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
