using System.Text.Json.Serialization;
using ChatSignalR.Models.WrapperChat;
using UserModel;

namespace UserLoginDTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? Email { get; set; }
        public Object Token { get; set; } = null;

        [JsonIgnore]
        public List<WrapperChat> OwnsChatIds { get; set; } = new List<WrapperChat>();

        public List<WrapperChat> MyChats { get; set; } = new List<WrapperChat>();
    }
}
