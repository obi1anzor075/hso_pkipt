using Microsoft.AspNetCore.Identity;

namespace HsoPkipt.Identity;

public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Position { get; set; } = "";
    public string? PhotoUrl { get; set; }
}
