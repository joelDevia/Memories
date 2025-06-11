using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Memories.Data;
using Memories.Models;
using Microsoft.AspNetCore.Mvc;

namespace Memories.Pages.Memories
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Memory> Memories { get; set; } = new();

        public int Page { get; set; } = 1;  // Página a mostrar

        public int TotalPages { get; set; }

        private const int PageSize = 9;

        public async Task OnGetAsync()
        {
            // Obtener el valor de Page de la URL si está presente
            var pageParam = Request.Query["Page"].ToString();
            if (int.TryParse(pageParam, out var page))
            {
                Page = page;
            }

            // Asegurarse de que Page no sea menor que 1
            if (Page < 1) Page = 1;

            // Obtener el total de recuerdos
            int totalMemories = await _context.Memories.CountAsync();

            // Calcular el total de páginas
            TotalPages = (int)Math.Ceiling(totalMemories / (double)PageSize);

            // Asegurarnos de que la página no sea mayor que el total de páginas
            if (Page > TotalPages) Page = TotalPages;

            // Obtener los recuerdos para la página actual
            Memories = await _context.Memories
                .OrderBy(m => m.Date)
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .Include(m => m.MediaFiles)
                .ToListAsync();
        }
    }
}
