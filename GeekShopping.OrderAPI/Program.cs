using GeekShopping.OrderAPI.MessageConsumer;
using GeekShopping.OrderAPI.Model.Context;
using GeekShopping.OrderAPI.RabbitMQSender;
using GeekShopping.OrderAPI.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

//Entity config
var connection = builder.Configuration["PostgressConnection:ConnectionString"];
builder.Services.AddDbContext<PostgressContext>(options => options.UseNpgsql(connection));

var dbBuilder = new DbContextOptionsBuilder<PostgressContext>();
dbBuilder.UseNpgsql(connection);

builder.Services.AddSingleton(new OrderRepository(dbBuilder.Options));

builder.Services.AddHostedService<RabbitMQCheckoutConsumer>();
builder.Services.AddHostedService<RabbitMQPaymentConsumer>();
builder.Services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();

builder.Services.AddControllers();

var app = builder.Build();

//Enable Legacy Timestamp for DateTime Fields in Postgress
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
