using HsoPkipt.Extensions;
using HsoPkipt.Identity;
using HsoPkipt.Models;
using HsoPkipt.Repositories;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services;
using HsoPkipt.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add db context
builder.Services.AddDb(builder.Configuration);

// Add identity
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Add Sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Repositories
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IMerchRepository, MerchRepository>();

// Add Services
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMerchService, MerchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

    // Проверка существования БД и применение миграций
    if (!await dbContext.Database.CanConnectAsync())
    {
        await dbContext.Database.MigrateAsync();
    }
    else
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await dbContext.Database.MigrateAsync();
        }
    }

    // Создание ролей
    string[] roles =
    [
        Roles.User,
        Roles.Moderator,
        Roles.Admin
    ];

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new AppRole { Name = role });
        }
    }

    // Дефолтный администратор
    string adminEmail = "admin@mail.ru";
    string adminPassword = "Admin123!";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var user = new AppUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, adminPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, Roles.Admin);
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(adminUser, Roles.Admin))
        {
            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
        }
    }
}

app.Run();