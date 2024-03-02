// UserModel
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using ChatModel;
using ChatSignalR.Models.WrapperChat;
using Enums;

namespace UserModel
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Pass { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gener { get; set; }

        public string? CurrentChatId { get; set; } = string.Empty;

        public string? CurrentConnectionId { get; set; } = string.Empty;

        public UserPermissionLevel UserPermissionLevel { get; set; } =
            UserPermissionLevel.CommonUser;

        public virtual ICollection<WrapperChat> MyOwnsChatIds { get; set; } =
            new List<WrapperChat>();

        public User(string userName, string email, string pass, string? gener)
        {
            UserName = userName;
            Email = email;
            Pass = pass;
            Gener = gener;
        }

        //to add a new chat at chat list <ids>
        public void AddChat(WrapperChat chat)
        {
            MyOwnsChatIds.Add(chat);
        }
    }
}
