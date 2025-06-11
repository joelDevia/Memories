using Memories.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar el DbContext con la cadena de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar los servicios necesarios para Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Configurar el middleware de la aplicación
app.UseRouting(); // Este middleware maneja las rutas

// Asegúrate de que las páginas Razor estén mapeadas correctamente
app.MapGet("/", context =>
{
    context.Response.Redirect("/Memories");
    return Task.CompletedTask;
});

app.MapRazorPages();

app.UseStaticFiles();

app.Run();
