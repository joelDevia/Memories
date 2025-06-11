using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Memories.Data;
using Memories.Models;
using Microsoft.EntityFrameworkCore;

namespace Memories.Pages.Memories
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Memory Memory { get; set; }

        public int Page { get; set; } = 1;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Capturamos el número de página directamente de la query string
            if (int.TryParse(HttpContext.Request.Query["page"], out var pageFromQuery))
            {
                Page = pageFromQuery;
            }

            Memory = await _context.Memories
                .Include(m => m.MediaFiles)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Memory == null)
            {
                return NotFound();
            }

            return Page();
        }
    }

}
