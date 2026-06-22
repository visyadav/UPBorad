namespace API.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
            ?? throw new InvalidOperationException("JwtSettings section is missing from configuration.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Key))
            };

            // options.Events = new JwtBearerEvents
            // {
            //     OnTokenValidated = async context =>
            //     {
            //         var roleRepository = context.HttpContext.RequestServices.GetRequiredService<Application.Interfaces.IRoleRepository>();
            //         
            //         var roleIdStr = context.Principal?.FindFirst("roleId")?.Value;
            //         var securityStampStr = context.Principal?.FindFirst("securityStamp")?.Value;
            //
            //         if (int.TryParse(roleIdStr, out var roleId) && Guid.TryParse(securityStampStr, out var stamp))
            //         {
            //             var role = await roleRepository.GetByIdAsync(roleId, context.HttpContext.RequestAborted);
            //             // If role doesn't exist, or security stamp changed, reject the token
            //             if (role == null || role.SecurityStamp != stamp)
            //             {
            //                 context.Fail("Token security stamp is invalid.");
            //             }
            //         }
            //         else
            //         {
            //             context.Fail("Token is missing required security stamp claims.");
            //         }
            //     }
            // };
        });

        return services;
    }
}