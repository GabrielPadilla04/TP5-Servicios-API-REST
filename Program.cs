using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TP5_Servicios_API_REST.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de la base de datos (MySQL con Pomelo)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 2. Configuración de Autenticación con Token JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "ClaveSecretaSuperSeguraTP5_2026!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "TP5Api",
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"] ?? "TP5Clients",
        ValidateLifetime = true
    };
});

// 3. Configuración de CORS en formato permisivo
builder.Services.AddCors(options =>
{
    options.AddPolicy("Permisivo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 4. Servicios de Controladores y OpenAPI / Swagger
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// 5. Pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("Permisivo");

// Habilitar Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();