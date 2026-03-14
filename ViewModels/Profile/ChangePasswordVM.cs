using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.ViewModels.Profile;

public class ChangePasswordVM
{
    [Display(Name = "Текущий пароль")]
    [Required(ErrorMessage = "Введите текущий пароль")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = "";

    [Display(Name = "Новый пароль")]
    [Required(ErrorMessage = "Введите новый пароль")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = "";

    [Display(Name = "Подтверждение нового пароля")]
    [Required(ErrorMessage = "Подтвердите новый пароль")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = "";
}