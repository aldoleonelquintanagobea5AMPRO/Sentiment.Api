using Microsoft.EntityFrameworkCore;
using Sentiment.Api.Models;
using Sentiment.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración DB: se recomienda pasar ConnectionStrings por variable de entorno en Docker.
// Ejemplo de key: ConnectionStrings__DefaultConnection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddSingleton<SentimentService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();