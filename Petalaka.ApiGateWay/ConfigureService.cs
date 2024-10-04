using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

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
            options.Cookie.Name = "Petalaka.Cookie";
        });
        services.AddAuthentication(options =>
        {
            /*            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;*/
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration.GetSection("JwtSettings:Issuer").Value,
                ValidAudience = configuration.GetSection("JwtSettings:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings:Key").Value)),
                ClockSkew = TimeSpan.Zero // No tolerance for token expiration
            };
            /*
            options.Events = new CustomJwtBearerEvents();
        */
        }).AddCookie(options =>
        {
            // Optional: Configure cookie settings if needed
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Always if running on HTTPS
            options.Cookie.SameSite = SameSiteMode.Lax; // Adjust based on your needs
            options.Cookie.Name = "Petalaka.Cookie";
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