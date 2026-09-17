using Microsoft.EntityFrameworkCore;
using TP5_Servicios_API_REST.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de la base de datos (MySQL con Pomelo)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 2. Servicios de Controladores y OpenAPI / Swagger
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// 3. Pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();