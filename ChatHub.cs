using ChatHubServices;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatHubChat
{
    public class ChatHub : Hub
{
    private readonly ChatService _chatService;
    private const string ReceiveMessageMethod = "ReceberMensagem";
    private const string UpdateUsersInChatMethod = "UpdateUsersInChat";

    public ChatHub(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HubMethodName("SendMessageToUser")]
    public async Task SendMessageToUser(string usuario, string mensagem, string chatId)
    {
        await Clients.Groups(chatId).SendAsync(ReceiveMessageMethod, usuario, mensagem);
    }


    public async Task JoinChat(string userId, string chatId)
    {
         if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(chatId))
    {
        await Clients.Caller.SendAsync("ErrorMessage", "Invalid userId or chatId");
        return ;
    }
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

        var usersInChat = _chatService.GetUsersInChat(chatId);
        await Clients.Group(chatId).SendAsync(UpdateUsersInChatMethod, usersInChat);

        await Clients.Group(chatId).SendAsync(ReceiveMessageMethod, $"{userId} joined the chat");
    }
}

}
