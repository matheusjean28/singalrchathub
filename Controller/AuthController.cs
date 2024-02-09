using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserContext;
using UserModel;
using UserLoginDTO;
using UserLoginModel;
using AuthServiceJwt;



namespace Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly AuthService _authJwt;

        public AuthController(UserDbContext context, AuthService authJwt)
        {
            _context = context;
            _authJwt  = authJwt;
        }

      

        [Route("CreateUser")]
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody]User user)
        {   
            if (await CheckIfUserExist(user))
            {
                var _userAlreadyExist = $"{user.UserName} Already exist";
                return Conflict(_userAlreadyExist);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var CreateUserOk = new UserDTO
                {   
                    Id = user.Id,
                    UserName= user.UserName,
                    Email= user.Email,
                    Token= null
                };

            Object messageResponse = new {Message ="User Created With success"};
            var _createdResponse = new {
                messageResponse,
                CreateUserOk
            };
            return Ok(_createdResponse);
        }

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
        public async Task<IActionResult> Auth([FromBody]UserLogin user)
        {
            try
            {
            var _searchedUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (_searchedUser  != null)
            {
                if( IsValidPassword(_searchedUser.Pass, user.Password))
                {

                var _userToken = new { token = _authJwt.GenerateJwtToken(_searchedUser) };
               
                var _responseAuthUserOk = new UserDTO
                {   
                    Id = _searchedUser.Id,
                    UserName= _searchedUser.UserName,
                    Email= _searchedUser.Email,
                    Token= _userToken
                };
                
                return Ok(_responseAuthUserOk);
                }
                else {
                    return BadRequest("Wrong Password!");
                }
            };

            var _notFound = $"{user.UserName} Was not Found, Does it realy exists?";
            return NotFound("_notFound");

            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex}");
                Console.WriteLine("Error:", ex.Message);
                return BadRequest("An error occurred during authentication."); 
            }
        }


        //move to other component after finish  
        public async Task<bool> CheckIfUserExist(User user)
        {
            return await _context.Users.AnyAsync(u => u.UserName == user.UserName);
        }

        public bool IsValidPassword(string paswordFromParams,  string passwordFromDb)
        {
            var isEqualPass = passwordFromDb.Trim().Equals(paswordFromParams.Trim());
            return isEqualPass;
        }
    }
}

    