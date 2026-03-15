using HsoPkipt.Models;
using Microsoft.EntityFrameworkCore;

public static class AddDbConnection
{
    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(
            options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Postgres"));
            }
            );
        return services;
    }
}