
using Application.Common.Services;
using Application.Interfaces.Common;
using Insfrastucture.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Insfrastucture;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IJwtTokenGenerate, JwtTokenGenerate>();
        services.AddSingleton<IPasswordHasher, PasswordHasherService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpClient<ITurnstileService, TurnstileService>();
        // services.AddScoped<IGrievanceService, GrievanceService>();
        services.AddScoped<ICommonServices, CommonServices>();
        // NOTE: ICacheService is registered in Program.cs (Redis or InMemory fallback)

        return services;
    }
}