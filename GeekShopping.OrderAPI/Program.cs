using GeekShopping.OrderAPI.MessageConsumer;
using GeekShopping.OrderAPI.Model.Context;
using GeekShopping.OrderAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:4435/";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "geek_shopping");
    });
});


var app = builder.Build();

//Enable Legacy Timestamp for DateTime Fields in Postgress
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
