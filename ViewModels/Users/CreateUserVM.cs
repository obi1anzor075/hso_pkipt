using HsoPkipt.Identity;

namespace HsoPkipt.ViewModels.Users;

public class CreateUserVM
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Position { get; set; }

    public string Role { get; set; } = Roles.User;
}