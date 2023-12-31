using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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

        [Route("Auth")]
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
             _context.Users.Add(user);
            await _context.SaveChangesAsync();

        return Ok(user.UserName);
        }

        [Route("getAllUser")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
        return await _context.Users.ToListAsync();
        }
    }
}
