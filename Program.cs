using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);



// Configurar autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("JwtBearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:7001",
            ValidAudience = "http://localhost:7001",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("2F2F408D-A34D-4C0A-ACE4-C6529E15237D"))
        };
    });

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange:true);



builder.Services.AddOcelot();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();


        });
});



var app = builder.Build();


app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication(); // Habilitar autenticación JWT
app.UseAuthorization();
await app.UseOcelot();
app.Run();
