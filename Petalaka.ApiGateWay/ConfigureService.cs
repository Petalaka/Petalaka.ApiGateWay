using Microsoft.OpenApi.Models;

namespace Petalaka.ApiGateWay;

public static class ConfigureService
{
    public static void AddConfigureService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddSingleton<SwaggerService>();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo() { Title = "API Gateway", Version = "v1" });
        });
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
        });
        services.AddAuthentication()
            .AddCookie(options =>
            {
                // Optional: Configure cookie settings if needed
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Always if running on HTTPS
                options.Cookie.SameSite = SameSiteMode.None; // Adjust based on your needs
            });
    }
}