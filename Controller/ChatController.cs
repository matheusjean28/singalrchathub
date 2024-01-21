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

                var _responseCreatedChat = $"Chat was Created with sucess!: {chatRoom}";

                return Ok(chatRoom);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            };
        }

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
