using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.ViewModels.Profile;

public class ProfileInfoVM
{
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = "";

    [Display(Name = "Фамилия")]
    public string LastName { get; set; } = "";

    [Display(Name = "Электронная почта")]
    [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты")]
    public string Email { get; set; } = "";

    [Display(Name = "Номер телефона")]
    [Phone(ErrorMessage = "Некорректный номер телефона")]
    public string PhoneNumber { get; set; } = "";

    [Display(Name = "Должность")]
    public string Position { get; set; } = "";
}