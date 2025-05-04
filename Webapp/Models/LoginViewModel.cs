using System.ComponentModel.DataAnnotations;
namespace Webapp.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Email", Prompt = "Enter Email")]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Password", Prompt = "Enter Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [Display(Name = "Terms", Prompt = "Confirm Terms")]
    public bool IsPersistent { get; set; }
}

