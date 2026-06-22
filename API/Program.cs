using API.Extensions;
using Application;
using Application.Interfaces;
using Insfrastucture;
using Persistence;
using Scalar.AspNetCore;
using API.Services;
using API.Filters;

var builder = WebApplication.CreateBuilder(args);


// ── Layer registrations ──────────────────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);


// ── JWT Bearer auth ──────────────────────────────────────────────────────────
builder.Services.AddJwtAuthentication(builder.Configuration);

// ── HttpContext accessor (needed by CurrentUserService) ──────────────────
builder.Services.AddHttpContextAccessor();

// ── Encryption Service ──────────────────────────────────────────────────────────
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();


// ── Redis (optional — falls back to in-memory cache if Redis is not running) ──
var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString");
var redisConnected = false;

if (!string.IsNullOrWhiteSpace(redisConnectionString))
{
    try
    {
        var redisOptions = new ConfigurationOptions
        {
            EndPoints = { redisConnectionString },
            ConnectTimeout = 2000,   // 2 s — fail fast in local dev
            AbortOnConnectFail = false,
        };
        var redis = ConnectionMultiplexer.Connect(redisOptions);

        if (redis.IsConnected)
        {
            builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
            builder.Services.AddSingleton<ICacheService, RedisCacheService>();
            redisConnected = true;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Startup] Redis unavailable ({ex.Message}). Falling back to in-memory cache.");
    }
}

if (!redisConnected)
{
    Console.WriteLine("[Startup] Using InMemoryCacheService (local dev mode — no Redis).");
    builder.Services.AddSingleton<ICacheService, InMemoryCacheService>();
}

// ── MVC / OpenAPI ────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<API.Filters.DynamicPermissionFilter>();
    })
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase);

builder.Services.AddOpenApi();


var app = builder.Build();

app.UseCors("AllowAll");

// ── HTTP pipeline ────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // UI available at /scalar
}

app.MapGet("/", () => "Server is running......");

app.UseGlobalExceptionMiddleware();
app.UseHttpsRedirection();

// ORDER IS CRITICAL: Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<EncryptionMiddleware>();

app.MapControllers();

app.Run();

public partial class Program;