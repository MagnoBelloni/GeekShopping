using GeekShopping.Email.MessageConsumer;
using GeekShopping.Email.Model.Context;
using GeekShopping.Email.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

//Entity config
var connection = builder.Configuration["PostgressConnection:ConnectionString"];
builder.Services.AddDbContext<PostgressContext>(options => options.UseNpgsql(connection));

var dbBuilder = new DbContextOptionsBuilder<PostgressContext>();
dbBuilder.UseNpgsql(connection);

builder.Services.AddSingleton(new EmailRepository(dbBuilder.Options));

builder.Services.AddScoped<IEmailRepository, EmailRepository>();

builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

var app = builder.Build();

//Enable Legacy Timestamp for DateTime Fields in Postgress
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseHttpsRedirection();

app.Run();
