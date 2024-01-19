using System.ComponentModel.DataAnnotations;

namespace CarForum.ViewModels;

public class ResetPasswordViewModel
{
    [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Email", Prompt = "Введите ваш Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Поле Старый пароль для заполнения")]
    [DataType(DataType.Password)]
    [Display(Name = "Старый пароль", Prompt = "Введите старый пароль")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "Поле Новый пароль для заполнения")]
    [DataType(DataType.Password)]
    [Display(Name = "Новый пароль", Prompt = "Придумайте новый пароль")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Обязательно подтвердите новый пароль")]
    [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердите пароль", Prompt = "Подтвердите новый пароль")]
    public string ConfirmNewPassword { get; set; }
}
