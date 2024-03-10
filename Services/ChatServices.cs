using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using ChatHubChat;
using ChatSignalR.Models.PermisionsChat;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserContext;
using UserModel;

namespace ChatHubServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatService : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger _logger;

        public ChatService(
            UserDbContext context,
            IHubContext<ChatHub> hubContext,
            ILogger<ChatService> logger
        )
        {
            _context = context;
            _hubContext = hubContext;
            _logger = logger;
        }

        // [Authorize(Policy = "Bearer")]
        [HttpGet("GetUsersInChat")]
        public async Task<ActionResult<List<User>>> GetUsersInChat(string chatId)
        {
            try
            {
                var _chatUsers = await _context
                    .Chats.Include(c => c.Users)
                    .FirstOrDefaultAsync(c => c.ChatID == chatId);

                if (_chatUsers == null)
                {
                    return NotFound($"Chat with ID {chatId} not found.");
                }
                var usersInChat = _chatUsers.Users.ToList();
                var loggerParams = string.Join(", ", usersInChat.Select(u => u.Id));
                _logger.LogInformation($"Users in Chat {chatId}: {loggerParams}");

                return Ok(usersInChat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("/UserIsAuthorizate")]
        public bool UserIsAuthorizate(string chatToken)
        {
            //check if token is null
            if (chatToken == "")
            {
                return false;
            }
            ;
            //create a table at database and save this temp token, check if that is valid
            return true;
        }

        [Authorize(Policy = "Bearer")]
        [HttpGet("/CheckUserCredentialsBeforeJoin")]
        public async Task<string> CheckUserCredentialsBeforeJoin(string UserId, string ChatId)
        {
            var user = await _context.Users.FindAsync(UserId);

            if (user == null)
            {
                return "UserID Not Found";
            }

            var chat = await _context.Chats.FindAsync(ChatId);

            if (chat == null)
            {
                return "ChatID Not Found";
            }

            return "OK";
        }

        //here we already check if that chat exists and at CheckIfUserIsAdminAndUserExist
        //we dont need to check it again
        [HttpGet("CheckIfChatAndUserExistAsync")]
        public async Task<bool> CheckIfChatAndUserExistAsync(string userIdToAdd, string ChatId)
        {
            //check if chat exist
            var chat = await _context.Chats.AnyAsync(chat => chat.ChatID == ChatId);
            var userIsAdmin = await _context.Chats.AnyAsync(chat =>
                chat.ChatID == ChatId && chat.OwnerId == userIdToAdd
            );

            if (chat)
            {
                return userIsAdmin;
            }
            else
            {
                return false;
            }
        }

        //check if user is an admin of the chat
        //and check if user to add is valid and if exists
        //protect with auth later
        [HttpGet]
        public async Task<bool> CheckIfUserIsAdminAndUserExist(string userId, string userToAdd)
        {
            var isAdmin = await _context.UserPermission.AnyAsync(permission =>
                permission.UserId == userId
                && permission.PermissionLevel == UserPermissionLevel.FullManeger
            );

            var userExist = await _context.Users.AnyAsync(user => user.Id == userToAdd);

            Console.WriteLine(isAdmin);
            Console.WriteLine(userExist);
            return isAdmin && userExist;
        }
    }
}
