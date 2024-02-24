using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ChatSignalR.Models.WrapperChat
{
    public class WrapperChat
    {
        [Key]
        public string Id {get;set;}

        [Required]
        public string ChatName {get;set;}
        
    }
}