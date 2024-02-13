using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;


using UserContext;
using ChatModel;
using UserModel;
using ChatBody;
using CreateDTO;

namespace Controllers
{
    public class ChatController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger _logger;

        public ChatController(UserDbContext context, ILogger<ChatController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        [Authorize]
        [HttpGet("GetAllRooms")]
        public async Task<ActionResult<List<Chat>>> GetAllRooms([FromHeader] string authorization)
        {
            _logger.LogInformation(authorization);
            var allRooms = await _context.Chats.ToListAsync();
            return Ok(allRooms);
        }

        [Authorize]
        [HttpPost("CreateRoom")]
        public async Task<ActionResult<CreateRoomModel>> CreateRoom([FromBody] CreateRoomModel model)
        {
            try
            {
                var user = await _context.Users.FindAsync(model.UserId);

                if (user == null)
                {
                    return BadRequest($"User with ID {model.UserId} not found.");
                }

                var chatRoom = new Chat
                {
                    ChatName = model.ChatName,
                    OnlineUser = model.OnlineUser,
                    Owner = user,
                    Users = new List<User> { user }
                };

                _context.Chats.Add(chatRoom);
                await _context.SaveChangesAsync();

                user.CurrentChatId = chatRoom.ChatID;
                await _context.SaveChangesAsync();

                //response dto with public data
                var _chatDTOresp = new CreateRoomDTO
                {
                    ChatID = chatRoom.ChatID,
                    ChatName = chatRoom.ChatName,
                    OnlineUser = chatRoom.OnlineUser,
                    Owner = user.Id,

                };
                var _responseCreatedChat = $"Chat was Created with sucess!: {_chatDTOresp}";

                return Ok(_chatDTOresp);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error ocurred + {ex}");
            };
                }

            [Authorize]
            [HttpDelete("Delete")]
                public async Task<ActionResult> DeleteRoom(string userId, string chat)
                {
                try
                {
                var user = await _context.Users.FindAsync("5d8c9046-0c60-4aba-b447-22879a0542cd");
                
                if (user == null)
                {
                    var userNotFoundMessage = $"UserId {userId} was not found. It is not possible to delete a room for a non-existent user.";
                    return NotFound(userNotFoundMessage);
                }

                var roomToDelete = await _context.Chats.FindAsync(chat);

                if (roomToDelete == null)
                {
                    var roomNotFoundMessage = $"Chat {chat} was not found. Room not deleted.";
                    return NotFound(roomNotFoundMessage);
                }

                _context.Chats.Remove(roomToDelete);
                await _context.SaveChangesAsync();

                return Ok("Room deleted successfully.");
                }
                catch (Exception ex)
                {
                var errorMessage = $"An error occurred: {ex.Message}";
                return BadRequest(errorMessage);
            }
        }



    }
}
