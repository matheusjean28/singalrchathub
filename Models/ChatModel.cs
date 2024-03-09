using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChatSignalR.Models.PermisionsChat;
using Enums;
using UserModel;

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

        public List<UserPermissionData> UserPermissionList { get; set; } =
        new List<UserPermissionData>();
    }
}
