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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway v1");
    
    c.SwaggerEndpoint("/swagger/account-service/v1/swagger.json", "Account Service");
    c.SwaggerEndpoint("/swagger/pet-store-service/v1/swagger.json", "PetStore Service");
    c.SwaggerEndpoint("/swagger/payment-service/v1/swagger.json", "Payment Service");
});
app.UseHttpsRedirection();
await app.UseOcelot();
app.Run();
