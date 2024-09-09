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
    }
}