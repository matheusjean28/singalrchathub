using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChatHubServices;
using MessageModel;
using Microsoft.AspNetCore.SignalR;

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
                if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(ChatId))
                {
                    await Clients.Caller.SendAsync("Error", "UserID or ChatID is invalid");
                }
                else
                {
                    var _joinMessage = await _chatService.CheckUserCredentialsBeforeJoin(
                        UserId,
                        ChatId
                    );
                    if (_joinMessage == "OK")
                    {
                        userConnections[UserId] = Context.ConnectionId;
                        await Groups.AddToGroupAsync(Context.ConnectionId, ChatId);
                        await Clients.Caller.SendAsync("JoinChat", "Joined With Sucess!");
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("Error", _joinMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Clients.Caller.SendAsync("Error", ex);
            }
        }

        public async Task SendMessageToGroup(
            string UserId,
            string ChatId,
            string Username,
            string UserMessage,
            string ChatToken
        )
        {
            try
            {
                if (_chatService.UserIsAuthorizate(ChatToken))
                {
                    if (IsValidMessage(UserMessage))
                    {
                        var _formatMessageToSend = new[]
                        {
                            new { User = Username, Message = UserMessage }
                        };

                        await Clients.Caller.SendAsync(
                            "SendMessageToGroup",
                            "Message sent successfully"
                        );
                        await Clients
                            .Groups(ChatId)
                            .SendAsync("ReceiveMessage", _formatMessageToSend);
                    }
                    else
                    {
                        await Clients.Caller.SendAsync("Error", "Message Cant be Empty");
                    }
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

        //broken this functions at another component before......
        public static bool IsValidMessage(string UserMessage)
        {
            return RegexStringMessage(UserMessage) != "String Null or Empty";
        }

        public static string RegexStringMessage(string UserMessage)
        {
            try
            {
                //check if string is null before start regex
                if (string.IsNullOrEmpty(UserMessage))
                {
                    return "String Null or Empty";
                }
                else
                {
                    string pattern = @"^[\p{L}\p{M}.,!\/\s]*$";
                    string placeReplace = "";
                    Regex regex = new Regex(pattern);

                    var _regexReturn = regex.Replace(UserMessage, placeReplace);

                    return _regexReturn;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("some error ocurred here" + ex);
                return "Regex Error: " + ex.Message;
            }
        }
    }
}
