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


        [HttpPost("CreateRoom")]
        public async Task<ActionResult<Chat>> CreateRoom(string ChatName, string Owner, int OnlineUser, string UserId)
        {

            var chatRoom = new Chat
            {
                ChatName = ChatName,
                OnlineUser = OnlineUser,
                // here need to check if is a valid user id, otherwise will throw an error
                UserId = "5d8c9046-0c60-4aba-b447-22879a0542cd"
            };

            _context.Chats.Add(chatRoom);
            await _context.SaveChangesAsync();

            return chatRoom;
        }


    }
}