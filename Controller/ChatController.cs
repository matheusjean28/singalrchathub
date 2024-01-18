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
                    Owner = user,
                    Users = new List<User> { user }
                };

                _context.Chats.Add(chatRoom);
                await _context.SaveChangesAsync();

                user.CurrentChatId = chatRoom.ChatID;
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
