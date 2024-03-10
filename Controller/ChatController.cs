using ChatBody;
using ChatHubServices;
using ChatModel;
using ChatSignalR.Dtos;
using ChatSignalR.Models.PermisionsChat;
using ChatSignalR.Models.WrapperChat;
using CreateDTO;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserContext;
using UserModel;

namespace Controllers
{
    public class ChatController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger _logger;
        private readonly ChatService _service;

        public ChatController(
            UserDbContext context,
            ILogger<ChatController> logger,
            ChatService service
        )
        {
            _context = context;
            _logger = logger;
            _service = service;
        }

        [HttpGet("GetAllAvaliableChats")]
        public async Task<ActionResult<IEnumerable<ChatResponseModelDTO>>> GetAllAvaliableChats()
        {
            try
            {
                var chats = await _context
                    .Chats.Include(c => c.UserPermissionList)
                    .Select(c => new ChatResponseModelDTO
                    {
                        ChatID = c.ChatID,
                        ChatName = c.ChatName,
                        OnlineUser = c.OnlineUser,
                        OwnerId = c.OwnerId,
                        UserPermissionList = c
                            .UserPermissionList.Select(up => new UserPermissionDataResponseModelDTO
                            {
                                Id = up.Id,
                                ChatID = up.ChatID,
                                UserId = up.UserId,
                                PermissionLevel = (int)up.PermissionLevel
                            })
                            .ToList()
                    })
                    .ToListAsync();

                return Ok(chats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Err4r on get cha, please try again later.");
            }
        }

        [HttpPost("/IncludeAdm")]
        public async Task<ActionResult<object>> IncludeAdm(
            string userClaimToAdd,
            string userIdToAdd,
            string ChatId,
            int UserLevel
        )
        {
            try
                {

                if (
                string.IsNullOrEmpty(userClaimToAdd) 
                || string.IsNullOrEmpty(userIdToAdd) 
                || string.IsNullOrEmpty(ChatId))
                {
                    return BadRequest("Invalid input. Please provide valid parameters.");
                }


                //if user exist and is owner cannot change, otherwise 
                //if chat exists and user are not owner, it can changed
                if(await _service.CheckIfChatAndUserExistAsync( userIdToAdd,  ChatId))
                {
                    return BadRequest("Onwer cannot have your level changed!, Or chat does not exists");
                }
                if (!await _service.CheckIfUserIsAdminAndUserExist(userClaimToAdd, userIdToAdd))
                {
                    //next check for errors
                    //if user level be bottom than 2 he cannot add other users.
                    return BadRequest(
                        "An error occurred! You may not have sufficient permissions or the user to be added does not exist."
                    );
                }

                //instance of current chat to save
                if (!CheckIfIsAValidEnum(UserLevel))
                {
                    return BadRequest("Invalid user level. Please provide a valid user level.");
                }

                var _chat = await _context.Chats.FindAsync(ChatId);

                var permission = new UserPermissionData
                {
                    UserId = userIdToAdd,
                    ChatID = ChatId,
                    PermissionLevel = (UserPermissionLevel)UserLevel 
                };

                _chat.UserPermissionList.Add(permission);
                await _context.SaveChangesAsync();
                return Ok(permission);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error was ocurred: {ex}");
            }
        }

        // [Authorize(Policy = "Bearer")]
        [HttpPost("CreateRoom")]
        public async Task<ActionResult<CreateRoomModel>> CreateRoom(
            [FromBody] CreateRoomModel model
        )
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

                //create a new userPermission instance and save them at database
                var permission = new UserPermissionData
                {
                    UserId = model.UserId,
                    PermissionLevel = UserPermissionLevel.FullManeger,
                };

                chatRoom.UserPermissionList.Add(permission);
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

                var _newWrapperChat = new WrapperChat
                {
                    Id = chatRoom.ChatID,
                    ChatName = chatRoom.ChatName,
                };
                //take user and add chat wrapper with name and id about that chat
                user.AddChat(_newWrapperChat);
                await _context.SaveChangesAsync();
                return Ok(_chatDTOresp);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
            ;
        }

        [Authorize(Policy = "Bearer")]
        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteRoom(string userId, string chat)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    var userNotFoundMessage =
                        $"UserId {userId} was not found. It is not possible to delete a room for a non-existent user.";
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

        //check if value of enum existing
        public bool CheckIfIsAValidEnum(int enumValue)
        {
            bool isValidEnum = Enum.IsDefined(typeof(UserPermissionLevel), enumValue);
            return isValidEnum;
        }
    }
}
