using System.Text;
using AdMeet.Attributes;
using AdMeet.Contexts;
using AdMeet.Models;
using AdMeet.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => { options.Filters.Add<Tracker>(); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar JWT Settings
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<Jwt>();
var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SK")!);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Environment.GetEnvironmentVariable("I"),
            ValidAudience = Environment.GetEnvironmentVariable("U"),
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(Environment.GetEnvironmentVariable("MYSQL_CONN"),
        ServerVersion.AutoDetect(Environment.GetEnvironmentVariable("MYSQL_CONN"))));

builder.Services.AddTransient<UserServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/

app.UseHttpsRedirection();

app.UseAuthentication(); // Habilitar la autenticación
app.UseAuthorization(); //

app.MapControllers();

app.Run();