using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserContext;
using ChatModel;
using UserModel;

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


        [HttpPost("CreateRoom")]
        public async Task<ActionResult<Chat>> CreateRoom(string chatName, int onlineUser, string userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return BadRequest($"User with ID {userId} not found.");
                }

                var chatRoom = new Chat
                {
                    ChatName = chatName,
                    OnlineUser = onlineUser,
                    Owner = user
                };

                _context.Chats.Add(chatRoom);
                await _context.SaveChangesAsync();

                return Ok(chatRoom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



    }
}
