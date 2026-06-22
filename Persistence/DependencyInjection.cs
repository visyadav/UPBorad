using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // Auto-detect provider: Postgres uses "Host=", SQL Server uses "Server="
            if (connectionString.Contains("Host=", StringComparison.OrdinalIgnoreCase))
                options.UseNpgsql(connectionString);
            else
                options.UseSqlServer(connectionString);
        });

        services.AddScoped<Application.Interfaces.IApplicationDbContext>(provider => 
            provider.GetRequiredService<ApplicationDbContext>());
            
        return services;
    }
}