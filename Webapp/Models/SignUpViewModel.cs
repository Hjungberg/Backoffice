using System.ComponentModel.DataAnnotations;

namespace Webapp.Models
{
    public class SignUpViewModel
    {
        [Required]
        [Display(Name = "First Name", Prompt = "Enter First Name")]
        public string FirstName { get; set; } = null!;
        [Required]
        [Display(Name = "Last Name", Prompt = "Enter Last Name")]
        public string LastName { get; set; } = null!;
        [Required]
        [Display(Name = "Email", Prompt = "Enter Email")]
        public string Email { get; set; } = null!;
        [Required]
        [Display(Name = "Password", Prompt = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        [Display(Name = "Terms", Prompt = "Confirm Terms")]

        public bool Terms { get; set; }
    }
}
