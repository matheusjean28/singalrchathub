using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserContext;
using ChatModel;
using UserModel;
using LoginModel;

namespace Controllers
{
    public class ChatController : ControllerBase
    {
        private readonly UserDbContext _context;

        public ChatController(UserDbContext context)
        {
            _context = context;
        }

        // [Route("GetRooms")]
        // [HttpGet]
        // public async Task IActionResult<Chat> GetAvaliableChats()
        // {
            
        // }
            // [Route("GetRooms")]
            // [HttpGet]
            // public List<Chat> List()
            // {
            //     List<Chat> rooms = new List<Chat>
            //     {
            //         new Chat {
            //             ChatID= "12asd1d5",ChatName = "room1", OnlineUser = 10, UserId= "1a2ds"},
            //     };

            //     return rooms;
            // }

        [HttpGet("GetAllRooms")]
        public async Task<ActionResult<List<Chat>>> GetAllRooms()
        {
        var allRooms = await _context.Chats.ToListAsync();
        return Ok(allRooms);
        }
        
    }
}
