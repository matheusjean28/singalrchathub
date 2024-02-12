using System.Threading.Tasks;
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

      

        [Route("/CreateUser")]
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody]UserModel.User user)
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

        var newUser = new User(user.UserName, user.Email, user.Pass, user.Gener);

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

                var _userToken = new { token = await _authJwt.GenerateJwtToken(user.UserName) };
               
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
            var userNameExist = await _context.Users.AnyAsync(u => 
            u.UserName == user.UserName );
            var userEmailExist = await _context.Users.AnyAsync(u => 
            u.Email == user.Email );
            
            if(userNameExist && userEmailExist){
                return true;
            }
            return false;

        }

        public bool IsValidPassword(string paswordFromParams,  string passwordFromDb)
        {
            var isEqualPass = passwordFromDb.Trim().Equals(paswordFromParams.Trim());
            return isEqualPass;
        }
    }
}

    