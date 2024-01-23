using CarForum.Models;
using System.ComponentModel.DataAnnotations;

namespace CarForum.ViewModels;

public class CreateCommentViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage ="Комментарий не может быть пустым")]
    [DataType(DataType.Text)]
    [Display(Name = "Комментарий", Prompt = "Напишите комментарий")]
    public string Text { get; set; }

    public DateTime PublishTime => DateTime.Now;
}
