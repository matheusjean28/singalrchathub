using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserContext;
using UserModel;
using UserLoginDTO;
using UserLoginModel;
namespace Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserDbContext _context;

        public AuthController(UserDbContext context)
        {
            _context = context;
        }

        [Route("getsomething")]
        [HttpGet]
        public string GetSomething()
        {
            return "something";
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
                var _userToken = new { token = DateTime.Now };
               
                var _responseAuthUserOk = new UserDTO
                {   
                    Id = _searchedUser.Id,
                    UserName= _searchedUser.UserName,
                    Email= _searchedUser.Email,
                    Token= _userToken
                };
                
                return Ok(_responseAuthUserOk);

            };

            var _notFound = $"{user.UserName} Was not Found, Does it realy exists?";
            return NotFound("_notFound");

            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error:", ex.Message);
                return BadRequest("An error occurred during authentication."); 
            }
        }


        //move to other component after finish  
        public async Task<bool> CheckIfUserExist(User user)
        {
            return await _context.Users.AnyAsync(u => u.UserName == user.UserName);
        }
    }
}

    