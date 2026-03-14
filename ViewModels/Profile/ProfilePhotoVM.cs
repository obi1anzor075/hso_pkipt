using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.ViewModels.Profile;

public class ProfilePhotoVM
{
    public string? CurrentPhotoUrl { get; set; }

    [Display(Name = "Фото профиля")]
    public IFormFile? Photo { get; set; }
}