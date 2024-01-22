using System.ComponentModel.DataAnnotations;

namespace CarForum.ViewModels;

public class FindReviewViewModel
{
    [Required(ErrorMessage = "Поле Бренд обязательно для заполнения")]
    [DataType(DataType.Text)]
    public string Brand { get; set; }

    [Required(ErrorMessage = "Поле Модель обязательно для заполнения")]
    [DataType(DataType.Text)]
    public string Model { get; set; }

    [Required(ErrorMessage = "Поле Год обязательно для заполнения")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Поле Тип Двигателя обязательно для заполнения")]
    [DataType(DataType.Text)]
    public string EngineType { get; set; }

    [Required(ErrorMessage = "Поле Кузов обязательно для заполнения")]
    [DataType(DataType.Text)]
    public string Body { get; set; }

    [Required(ErrorMessage = "Поле Коробка передач обязательно для заполнения")]
    [DataType(DataType.Text)]
    public string Transmission { get; set; }
}
