namespace Petalaka.ApiGateWay;

public class SwaggerService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SwaggerService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> GetSwaggerJson(string url)
    {
        var client = _httpClientFactory.CreateClient();
        return await client.GetStringAsync(url);
    }
}