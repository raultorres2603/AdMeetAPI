using System.Text;
using AdMeet.Contexts;
using AdMeet.Inter;
using AdMeet.Models;
using AdMeet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar JWT Settings
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("JwtSettings"));

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

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

builder.Services.AddTransient<IJwt, Jwt>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IKpiService, KpiService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin() // Reemplaza con el dominio permitido
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication(); // Habilitar la autenticación
app.UseAuthorization(); //
app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();