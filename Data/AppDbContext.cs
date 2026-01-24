using HsoPkipt.Identity;
using HsoPkipt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Data;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }

    public DbSet<NewsItem> News => Set<NewsItem>();
    public DbSet<Tag> Tags => Set<Tag>();
}
