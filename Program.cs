using Control.Data;
using Control.ControlMapper.Profiles;
using Microsoft.EntityFrameworkCore;
using Control.Repositories.Implementations;
using Control.Repositories.Interfaces;
using Control.Services.Implementations;
using Control.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ==================== CONFIGURACI�N DE SERVICIOS ====================

// Configurar Entity Framework con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL")));

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(PersonaProfile));

// Registrar Repositorios
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();

// Registrar Servicios
builder.Services.AddScoped<IPersonaService, PersonaService>();

// Agregar servicios de controladores
builder.Services.AddControllers();

// Agregar servicios de MVC (si usas Views)
builder.Services.AddControllersWithViews();

// Configurar API Explorer para Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS si es necesario
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// ==================== CONSTRUCCI�N DE LA APLICACI�N ====================

var app = builder.Build();

// ==================== CONFIGURACI�N DEL PIPELINE ====================

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Usar CORS si se configur�
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

// Configurar rutas para controladores
app.MapControllers();

// Configurar ruta por defecto si usas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ==================== INICIALIZACI�N DE BASE DE DATOS ====================

// Opcional: Aplicar migraciones autom�ticamente al iniciar
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        // Aplicar migraciones pendientes
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log del error - puedes usar ILogger aqu�
        Console.WriteLine($"Error al aplicar migraciones: {ex.Message}");
    }
}

// ==================== EJECUTAR LA APLICACI�N ====================

app.Run();