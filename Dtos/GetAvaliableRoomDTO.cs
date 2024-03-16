using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSignalR.Dtos
{
    public class ChatResponseModelDTO
    {
        public string ChatID { get; set; }
        public string ChatName { get; set; }
        public int OnlineUser { get; set; }
        public string OwnerId { get; set; }
        public List<UserPermissionDataResponseModelDTO> UserPermissionList { get; set; }
    }

    public class UserPermissionDataResponseModelDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ChatID { get; set; }
        public int PermissionLevel { get; set; }
    }
}
