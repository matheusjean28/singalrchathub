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

    public async Task SendMessageToGroup (string UserId, string ChatId, string UserMessage, string ChatToken)
    {
        try
        {
         if(_chatService.UserIsAuthorizate(ChatToken))
         {
        await Clients.Caller.SendAsync("SendMessageToGroup", "Message sent successfully");

        await Clients.All.SendAsync("ReciveMessage" ,UserMessage);

         }
         else
         {
            await Clients.Caller.SendAsync("Error", "Username not Allow at this Chat");
         }
        }
        catch (Exception error)
        {
            Console.WriteLine(error);

            await Clients.Caller.SendAsync("Error", error);
        }


    }

    }
    }
