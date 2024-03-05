using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserModel;
using Enums;

namespace ChatModel
{
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ChatID { get; set; }

        [Required(ErrorMessage = "ChatName is required")]
        public string ChatName { get; set; }

        //if not declared, default value is "10"
        public int OnlineUser { get; set; } = 10;

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public User Owner { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public List<UserPermission> UserPermissions { get; set; } 

        public List<UserPermission> GetUserPermissions()
        {
            return UserPermissions;
        }
    }

    public class UserPermission
    {
        public string UserId { get; set; }
        public UserPermissionLevel PermissionLevel { get; set; } = 0;
    }
}
