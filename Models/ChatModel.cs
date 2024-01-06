using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        //if not declared, defoult value is "10"
        public int OnlineUser { get; set; } = 10;

        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
