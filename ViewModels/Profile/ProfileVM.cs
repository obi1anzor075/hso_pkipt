namespace HsoPkipt.ViewModels.Profile;

public class ProfileVM
{
    public ProfileInfoVM ProfileInfo { get; set; } = new();

    public ProfilePhotoVM ProfilePhoto { get; set; } = new();

    public ChangePasswordVM ChangePassword { get; set; } = new();

    public List<EventVM> UpcomingEvents { get; set; } = new();
}