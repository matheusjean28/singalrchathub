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


        //move to other component after finish  
        public async Task<bool> CheckIfUserExist(User user)
        {
            
              return await _context.Users.AnyAsync(u => u.UserName == user.UserName);

        }
        
    }
}
