using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Memories.Data;
using Memories.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Memories.Pages.Memories
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

        [BindProperty]
        public Memory Memory { get; set; } = new();

        [BindProperty]
        public List<IFormFile>? Files { get; set; } = new(); // Permitir lista vacía de archivos

        public IActionResult OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Memories.Add(Memory);
            await _context.SaveChangesAsync();

            if (Files != null && Files.Count > 0)
            {
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadPath);

                foreach (var file in Files)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(uploadPath, fileName);

                    using (var stream = System.IO.File.Create(fullPath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Determinar el tipo de archivo
                    var extension = Path.GetExtension(file.FileName).ToLower();
                    MediaType type;

                    if (new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }.Contains(extension))
                        type = MediaType.Image;
                    else if (new[] { ".mp4", ".mov", ".avi", ".wmv", ".webm" }.Contains(extension))
                        type = MediaType.Video;
                    else if (new[] { ".mp3", ".wav", ".ogg", ".m4a" }.Contains(extension))
                        type = MediaType.Audio;
                    else
                        continue; // Salta archivos con extensión desconocida

                    var mediaFile = new MediaFile
                    {
                        FilePath = $"/uploads/{fileName}",
                        Type = type,
                        MemoryId = Memory.Id
                    };

                    _context.MediaFiles.Add(mediaFile);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Memories/Index");
        }

    }
}
