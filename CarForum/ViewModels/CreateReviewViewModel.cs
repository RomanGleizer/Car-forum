using CarForum.Models;
using System.ComponentModel.DataAnnotations;

namespace CarForum.ViewModels;

public class CreateReviewViewModel
{
    [Required(ErrorMessage = "Поле Название отзыва обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Название отзыва", Prompt = "Придумайте название для отзыва")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Поле Рейтинг обязательно для заполнения")]
    [Display(Name = "Ваша оценка автомобилю по 5-бальной шкале", Prompt = "Поставьте оценку автомобилю")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "Поле Марка обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Марка автомобиля", Prompt = "Укажите марку автомобиля")]
    public string CarBrand { get; set; }

    [Required(ErrorMessage = "Поле Модель обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Модель автомобиля", Prompt = "Укажите модель автомобиля")]
    public string CarModel { get; set; }

    [Required(ErrorMessage = "Поле Коробка передач обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Коробка передач", Prompt = "Укажите коробку передач автомобиля")]
    public string Transmission { get; set; }

    [Required(ErrorMessage = "Поле Тип кузова обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Тип кузова", Prompt = "Укажите тип кузова автомобиля")]
    public string Body { get; set; }

    [Required(ErrorMessage = "Поле Год производства обязательно для заполнения")]
    [Display(Name = "Укажите год производства автомобиля", Prompt = "Год производства автомобиля")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Поле Пробег обязательно для заполнения")]
    [Display(Name = "Укажите текущий пробег автомобиля в км", Prompt = "Текущий пробег автомобиля в км")]
    public int Mileage { get; set; }

    [Required(ErrorMessage = "Поле Тип двигателя обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Тип двигателя", Prompt = "Укажите тип двигателя автомобиля")]
    public string EngineType { get; set; }

    [Required(ErrorMessage = "Поле Объем двигателя обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Объем двигателя", Prompt = "Укажите объем двигателя автомобиля")]
    public string EngineCapacity { get; set; }

    [Required(ErrorMessage = "Поле Привод обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Привод", Prompt = "Укажите тип привода вашего автомобиля")]
    public string DriveType { get; set; }

    [Required(ErrorMessage = "Поле Основые приемущества обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Основые приемущества", Prompt = "Укажите основые приемущества вашего автомобиля")]
    public string Advantages { get; set; }

    [Required(ErrorMessage = "Поле Основые недостатки обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Основые недостатки", Prompt = "Укажите основые недостатки вашего автомобиля")]
    public string Disadvantages { get; set; }

    [Required(ErrorMessage = "Поле Общее мнение обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Общее мнение", Prompt = "Подробно распишите опыт владения автомобилем(обслуживание, эксплуатация и так далее...)")]
    public string OverallExperience { get; set; }

    [Required(ErrorMessage = "Поле Фотографии обязательно для заполнения")]
    [DataType(DataType.Upload)]
    [Display(Name = "Фотография автомобиля", Prompt = "Добавьте фотографию автомобиля")]
    public IFormFile? Photo { get; set; }

    public DateTime PublishTime => DateTime.Now;
}
