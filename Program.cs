using BikeBuster.Data;
using BikeBuster.Messaging.Consumers;
using BikeBuster.Models;
using BikeBuster.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do PostgreSQL
// Usa ConnectionStrings__Postgres do docker-compose ou DefaultConnection do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("Postgres") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(connectionString));

// Configuração do MassTransit com RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BikeCreatedConsumer>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        // Lê as configurações do RabbitMQ das variáveis de ambiente ou appsettings.json
        var rabbitHost = builder.Configuration["RabbitMq:Host"] ?? "localhost";
        var rabbitUser = builder.Configuration["RabbitMq:Username"] ?? "guest";
        var rabbitPass = builder.Configuration["RabbitMq:Password"] ?? "guest";
        
        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username(rabbitUser);
            h.Password(rabbitPass);
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddScoped<BikeService>();
builder.Services.AddScoped<RentalService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();