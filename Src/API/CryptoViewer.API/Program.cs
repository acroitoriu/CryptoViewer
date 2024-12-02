using CryptoViewer.API.Middleware;
using CryptoViewer.Application;
using CryptoViewer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExchangeRatesApiExceptionsHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();

app.Run();

//needed for IntegrationTests
public partial class Program { }