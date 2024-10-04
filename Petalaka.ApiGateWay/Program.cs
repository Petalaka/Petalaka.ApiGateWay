using Microsoft.AspNetCore.HttpOverrides;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Petalaka.ApiGateWay;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddConfigureService(builder.Configuration);
var app = builder.Build();
app.UseCors("AllowAll");
app.UseRouting();
app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
    
    c.SwaggerEndpoint("/swagger/order/v1/swagger.json", "Order Service");
    c.SwaggerEndpoint("/swagger/account/v1/swagger.json", "Account Service");

});
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
await app.UseOcelot();
app.Run();
