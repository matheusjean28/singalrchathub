// UserModel
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

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


    }
}
