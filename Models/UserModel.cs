using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserModel
{
    public class User
    {   [Key ]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public required string Id {get;set;}

        [Required(ErrorMessage = "UserName is required")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public required string Pass { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public required string Gener { get; set; }
    }
}
