using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Enums;

namespace ChatSignalR.Models.PermisionsChat
{
    public class UserPermissionData
    {
        //id navigation to get this item
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserName { get; set; }

        [ForeignKey("ChatID")]
        public string ChatID { get; set; }

        //user id that are in chat
        public string UserId { get; set; }

        //your permission level
        public UserPermissionLevel PermissionLevel { get; set; } = 0;
    }
}
