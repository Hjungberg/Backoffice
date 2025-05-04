using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class SignUpViewModel
{
    [Required]
    [Display(Name = "First Name", Prompt = "Enter First Name")]
    public string FirstName { get; set; } = null!;
    [Required]
    [Display(Name = "Last Name", Prompt = "Enter Last Name")]
    public string LastName { get; set; } = null!;
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
    [Required]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    [Display(Name = "Comfirm password", Prompt = "Comfirm password")]
    public string ConfirmPassword { get; set; } = null!;
    [Range(typeof(bool), "true", "true")]
    public bool TermsAndConditions { get; set; } 

}
