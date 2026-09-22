using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using TP5_Servicios_API_REST.Data;
using TP5_Servicios_API_REST.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Base de datos (MySQL con versión fija para evitar crasheos de arranque en MonsterASP)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

// 2. Autenticación JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "ClaveSecretaSuperSeguraTP5_2026!ConSuficienteLongitud12345");

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

// 3. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Permisivo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 4. Inyección de Dependencias
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ImagenService>();

// 5. Controladores y OpenAPI NATIVO de .NET
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// 6. Pipeline HTTP (Modificado para funcionar en producción de MonsterASP)
app.MapOpenApi();
app.MapScalarApiReference();

// Redirecciona automáticamente la raíz (/) directamente hacia la interfaz de Scalar
app.MapGet("/", () => Results.Redirect("/scalar/v1"));

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("Permisivo");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
