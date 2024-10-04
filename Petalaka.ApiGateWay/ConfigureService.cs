using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

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
        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromHours(1); // Thời gian hết hạn của session
            options.Cookie.HttpOnly = true; // Không cho phép JavaScript truy cập cookie
            options.Cookie.IsEssential = true; // Để sử dụng session cần thiết
        });
        services.AddAuthentication()
            .AddCookie(options =>
            {
                // Optional: Configure cookie settings if needed
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Always if running on HTTPS
                options.Cookie.SameSite = SameSiteMode.None; // Adjust based on your needs
            })
          .AddGoogle(options =>
          {
              options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
              options.ClientId = configuration.GetSection("GoogleSettings:ClientId").Value;
              options.ClientSecret = configuration.GetSection("GoogleSettings:ClientSecret").Value;
              options.Scope.Add("email");
              options.Scope.Add("profile");
              options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
              options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
          }); 
    }
}