using System.ComponentModel.DataAnnotations;

namespace CarForum.ViewModels;

public class EditReviewViewModel
{
    public int Id { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Название отзыва", Prompt = "Название для отзыва")]
    public string? Title { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Оценка автомобилю по 5-бальной шкале", Prompt = "Оценка автомобилю")]
    public int Rating { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Марка автомобиля", Prompt = "Марка автомобиля")]
    public string? CarBrand { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Модель автомобиля", Prompt = "Модель автомобиля")]
    public string? CarModel { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Коробка передач", Prompt = "Коробка передач автомобиля")]
    public string? Transmission { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Тип кузова", Prompt = "Тип кузова автомобиля")]
    public string? Body { get; set; }

    [Display(Name = "Год производства автомобиля", Prompt = "Год производства автомобиля")]
    public int Year { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Тип двигателя", Prompt = "Тип двигателя автомобиля")]
    public string EngineType { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Объем двигателя", Prompt = "Объем двигателя автомобиля")]
    public string EngineCapacity { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Привод", Prompt = "Тип привода")]
    public string DriveType { get; set; }

    [Display(Name = "Текущий пробег автомобиля в км", Prompt = "Текущий пробег автомобиля в км")]
    public int Mileage { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Приемущества", Prompt = "Основые приемущества вашего автомобиля")]
    public string? Advantages { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Недостатки", Prompt = "Основые недостатки вашего автомобиля")]
    public string? Disadvantages { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Общее мнение", Prompt = "Общее мнение")]
    public string? OverallExperience { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Фотография автомобиля", Prompt = "Добавьте фотографию автомобиля")]
    public IFormFile? Photo { get; set; }

    [DataType(DataType.Text)]
    public string? PhotoPath { get; set; }
}
