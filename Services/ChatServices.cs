using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UserContext;
using ChatHubChat;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.Arm;
using UserModel;
using Microsoft.EntityFrameworkCore;

namespace ChatHubServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatService : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(UserDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost("JoinChat")]
public async Task<ActionResult> JoinChat(string userId, string chatId)
{
    var user = await _context.Users.FindAsync(userId);

    if (user == null)
    {
        return BadRequest($"User with ID {userId} not found.");
    }

    user.CurrentChatId = chatId;
    await _context.SaveChangesAsync();

    await _hubContext.Clients.Client(user.CurrentConnectionId).SendAsync("ReceiveMessage", "You joined the chat");
    await _hubContext.Clients.Group(chatId).SendAsync("ReceiveMessage", $"{userId} joined the chat");

    await _hubContext.Groups.AddToGroupAsync(user.CurrentConnectionId, chatId);

    return Ok();
}


        [HttpPost("SendMessage")]
        public async Task<ActionResult> SendMessage(string userId, string chatId, string message)
        {
            await _hubContext.Clients.Group(chatId).SendAsync("ReceiveMessage", $"{userId}: {message}");
            return Ok();
        }

     [HttpGet("GetUsersInChat")]
public async Task<ActionResult<List<User>>> GetUsersInChat(string chatId)
{
    try
    {
        var chat = await _context.Chats
            .Include(c => c.Users) 
            .FirstOrDefaultAsync(c => c.ChatID == chatId);

        if (chat == null)
        {
            return NotFound($"Chat with ID {chatId} not found.");
        }

        var usersInChat = chat.Users.ToList();
        return Ok(usersInChat);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal Server Error: {ex.Message}");
    }
}


    }
}
