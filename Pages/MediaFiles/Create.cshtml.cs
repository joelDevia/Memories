using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Memories.Data;
using Memories.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Memories.Pages.MediaFiles
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CreateModel(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty(SupportsGet = true)]
        public int MemoryId { get; set; }

        [BindProperty]
        public List<IFormFile> Files { get; set; } = new();

        [BindProperty]
        public MediaType Type { get; set; }

        public IActionResult OnGet()
        {
            if (!_context.Memories.Any(m => m.Id == MemoryId))
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Files.Count == 0)
                return Page();

            var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadPath); // Crea el directorio si no existe

            foreach (var file in Files)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(uploadPath, fileName);

                using (var stream = System.IO.File.Create(fullPath))
                {
                    await file.CopyToAsync(stream);  // Carga el archivo en el servidor
                }

                // Agregar la ruta del archivo cargado en la base de datos
                var mediaFile = new MediaFile
                {
                    FilePath = $"/uploads/{fileName}",
                    Type = Type,
                    MemoryId = MemoryId
                };

                _context.MediaFiles.Add(mediaFile);  // Agregar a la base de datos
            }

            await _context.SaveChangesAsync();  // Guardar cambios en la base de datos

            return RedirectToPage("/Memories/Index"); // Redirigir a la página de recuerdos
        }
    }
}
