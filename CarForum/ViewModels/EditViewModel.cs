using System.ComponentModel.DataAnnotations;

namespace CarForum.ViewModels;

public class EditViewModel
{
    [DataType(DataType.Text)]
    [Display(Name = "Имя", Prompt = "Имя")]
    public string? FirstName { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Фамилия", Prompt = "Фамилия")]
    public string? LastName { get; set; }

    [EmailAddress]
    [Display(Name = "Email", Prompt = "Email")]
    public string? Email { get; set; }

    [Phone]
    [Display(Name = "Телефон", Prompt = "Телефон")]
    public string? Phone { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Страна", Prompt = "Страна")]
    public string? Country { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Город", Prompt = "Город")]
    public string? City { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "День Рождения", Prompt = "День Рождения")]
    public DateTime BirthDay { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Опыт вождения", Prompt = "Опыт вождения")]
    public int DrivingExperienceYears { get; set; }

    public string? Login => Email;
}
