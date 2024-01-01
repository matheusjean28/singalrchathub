using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserContext;
using UserModel;

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
            return "alguma coisa";
        }

        [Route("CreateUser")]
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
        
        if(await CheckIfUserExist(user))
        {
            var _userAlreadyExist = $"{user.UserName} Already exist";
            return Conflict(_userAlreadyExist);
        }
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var _userCreated = $"{user.UserName} was created with sucess!";
        return Ok( _userCreated);
        }

        [Route("getAllUser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
        return await _context.Users.ToListAsync();
        }

        [Route("Auth")]
        [HttpPost]
        public async Task<IActionResult> Auth(User user)
        {   
            var _searchedUser = await _context.Users.AnyAsync(u => u.UserName == user.UserName); 
            Debug.Print($"_searchedUser: {_searchedUser}");

            if(_searchedUser)
            {
            var _userToken = $"{DateTime.Now + user.Id} {user.Id}";
            Console.WriteLine("User found. Authorized.");
            return Ok(_userToken);
            
            };

            var _notFound = $"{user.UserName} Was not Found, Does it realy exists?";
            return BadRequest(_notFound);
        }


        //move to other component after finish  
        public async Task<bool> CheckIfUserExist(User user)
        {
              return await _context.Users.AnyAsync(u => u.UserName == user.UserName);
        }
        
    }
}
