using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class SignInViewModel
{
    [Required]
    [RegularExpression(@"")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter your Email")]
    public string Email { get; set; } = null!;
    [Required]
    [RegularExpression(@"")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Password")]
    public string Password { get; set; } = null!;

    public bool IsPersistent { get; set; }
}
