using System.ComponentModel.DataAnnotations;

namespace CarForum.ViewModels;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email", Prompt = "Введите email")]
    public string? UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]

    [Display(Name = "Пароль", Prompt = "Введите пароль")]
    public string? Password { get; set; }

    [Display(Name = "Запомнить меня?")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}
