using System.ComponentModel.DataAnnotations;


namespace LoginModel
{
    public class UserLoginModel
    {
                [Required(ErrorMessage = "UserName is required")]
                public required string UserName { get; set; }

                [Required(ErrorMessage = "Password is required")]
                public required string Password { get; set; }
                
    }
    
}