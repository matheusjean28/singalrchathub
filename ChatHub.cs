using ChatHubServices;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatHubChat
{
    public class ChatHub : Hub
{
    private readonly ChatService _chatService;

    public ChatHub(ChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task SendMessageToUser(string userId ,string user, string message,string chatId)
    {
    try
    {
        await Groups.AddToGroupAsync(userId, chatId);
        await Clients.All.SendAsync("SendMessageToUser", user, message);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Error: {ex.Message}");
        await Clients.Caller.SendAsync("ErrorMessage", $"Failed to send message: {ex.Message}");
    }
    }

     public async Task ReciveMessage(string name, string message)
        {
            await Clients.All.SendAsync("reciveMessage", name, message);
        }



       public async Task JoinChat(string userId, string chatId)
            {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(chatId))
            {
                await Clients.Caller.SendAsync("ErrorMessage", "Invalid userId or chatId");
                return;
            }

            await Groups.AddToGroupAsync(userId, chatId);

            await Clients.Group(chatId).SendAsync("SendMessageToUser", $"{userId} joined the chat");

            var usersInChat = _chatService.GetUsersInChat(chatId);
            await Clients.Group(chatId).SendAsync("UpdateUsersInChatMethod", usersInChat);
            }


    
}

}
