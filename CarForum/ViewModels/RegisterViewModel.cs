﻿using System.ComponentModel.DataAnnotations;

namespace CarForum.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Имя", Prompt = "Введите имя")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
    [EmailAddress]
    [Display(Name = "Email", Prompt = "Введите ваш email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Поле Телефон обязательно для заполнения")]
    [Phone]
    [Display(Name = "Телефон", Prompt = "Введите ваш телефон")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
    [DataType(DataType.Password)]

    [Display(Name = "Пароль", Prompt = "Введите пароль")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Обязательно подтвердите пароль")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль", Prompt = "Введите пароль повторно")]
    public string? PasswordConfirm { get; set; }

    [Required(ErrorMessage = "Поле Страна обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Страна", Prompt = "Введите вашу страну")]
    public string? Country { get; set; }

    [Required(ErrorMessage = "Поле Город обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Город", Prompt = "Введите ваш город")]
    public string? City { get; set; }

    [Required(ErrorMessage = "Поле День обязательно для заполнения")]
    [Display(Name = "День", Prompt = "День")]
    public int Day { get; set; }

    [Required(ErrorMessage = "Поле Месяц обязательно для заполнения")]
    [Display(Name = "Месяц", Prompt = "Месяц")]
    public string Month { get; set; }

    [Required(ErrorMessage = "Поле Год обязательно для заполнения")]
    [Display(Name = "Год", Prompt = "Год")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Поле Опыт вождения обязательно для заполнения")]
    [DataType(DataType.Text)]
    [Display(Name = "Опыт вождения", Prompt = "Введите ваш опыт вождения")]
    public int DrivingExperienceYears { get; set; }

    public DateTime RegistrationDate => DateTime.Now;

    public string? Login => Email;
}
