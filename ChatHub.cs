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

        public async Task EnviarMensagem(string usuario, string mensagem)
        {
            await Clients.All.SendAsync("ReceberMensagem", usuario, mensagem);
        }

        public async Task ReceberMensagem(string usuario, string mensagem)
        {
            await Clients.All.SendAsync("ReceberMensagem", usuario, mensagem);
        }

public async Task JoinChat(string userId, string chatId)
{
    await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

    var usersInChat = _chatService.GetUsersInChat(chatId);
    await Clients.Group(chatId).SendAsync("UpdateUsersInChat", usersInChat);

    await Clients.Group(chatId).SendAsync("ReceiveMessage", $"{userId} joined the chat");
}

    }
}
