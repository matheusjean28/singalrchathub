    using ChatHubServices;
    
    using MessageModel;
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
        private Dictionary<string, string> userConnections = new Dictionary<string, string>();

        //send a req to join at chat
        public async Task JoinChat(string UserId, string ChatId)
        {
            try
            {
                if(string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(ChatId) )
                {
                await Clients.Caller.SendAsync("Error", "UserID or ChatID is invalid");
                }else 
                {
                var _joinMessage = await _chatService.CheckUserCredentialsBeforeJoin(UserId, ChatId);
                if( _joinMessage == "OK")
                {
                    userConnections[UserId] = Context.ConnectionId;
                    await Groups.AddToGroupAsync(Context.ConnectionId, ChatId);
                    await Clients.Caller.SendAsync("JoinChat", "Joined With Sucess!");
                }
                else 
                {
                    await Clients.Caller.SendAsync("Error", _joinMessage  );
                }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Clients.Caller.SendAsync("Error", ex);
            }
        }


            public async Task SendMessageToGroup(string UserId, string ChatId, Object UserMessage, string ChatToken)
            {
                try
                {
                    if (_chatService.UserIsAuthorizate(ChatToken))
                    {
                        await Clients.Caller.SendAsync("SendMessageToGroup", "Message sent successfully");
                        await Clients.Groups(ChatId).SendAsync("ReceiveMessage", UserMessage);
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("Error", "Username not allowed in this Chat");
                    }
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                    await Clients.Caller.SendAsync("Error", "An error occurred");
                }
            }


    }
    }
