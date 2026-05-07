using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.DTO;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBContext _context;

        public HomeController(DBContext context)
        {
            _context = context;
        }

        [HttpGet("status-summary")]
        public async Task<ActionResult> Index()
        {
            var result = await _context.Projects
                .Select(p => new
                {
                    Type = p.ProjectType,
                    Name = p.Trackings
                        .OrderByDescending(t => t.createdOn)
                        .Select(t => t.Status!.Name)
                        .FirstOrDefault()
                })
                .GroupBy(x => new
                {
                    x.Type,
                    Name = x.Name ?? "No Status"
                })
                .Select(g => new
                {
                    Type = g.Key.Type.ToString(),
                    Name = g.Key.Name,
                    Totals = g.Count()
                })
                .OrderBy(x => x.Type)
                .ThenBy(x => x.Name)
                .ToListAsync();

            return Ok(result);
        }
    }
}
