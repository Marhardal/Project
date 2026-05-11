using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.DTO;
using Project.Migrations;
using Project.Models;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBContext _context;

        public HomeController(DBContext context)
        {
            _context = context;
        }

        [HttpGet("api/status-summary")]
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

        // GET: api/Proponents
        [HttpGet("api/GetProjectbyProponent")]
        public async Task<ActionResult<IEnumerable<ProjectbyProponentDTO>>> GetProjectbyProponent()
        {
            return await _context.Projects.Where(p => p.Proponent != null).GroupBy(p => p.Proponent.Name).OrderByDescending(p => p.Count())
            .Select(g => new ProjectbyProponentDTO
            {
                Proponent = g.Key!,
                Count = g.Count()
            }).Take(5).ToListAsync();
        }


        [HttpGet("api/GetRecentProjects")]
        public async Task<ActionResult<IEnumerable<RecentActivityDTO>>> GetRecentProject()
        {
            var trackings = await _context.Trackings
                .Include(t => t.Project)
                    .ThenInclude(p => p.Proponent)
                .Include(t => t.Status)
                .OrderByDescending(t => t.createdOn)
                .Take(50)
                .ToListAsync();

            var result = trackings
                .GroupBy(t => t.ProjectID)
                .Select(g => g.First())
                .Take(5)
                .Select(t => new RecentActivityDTO
                {
                    Project = t.Project.Name,
                    Proponent = t.Project.Proponent.Name,
                    Status = t.Status.Name,
                    Color = t.Status.Color,
                    Date = t.createdOn
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("api/GetGroupedProjects")]
        public async Task<ActionResult<IEnumerable<GroupedProjectTypesDTO>>> GetGroupedProjects()
        {
            var result = await _context.Projects
                .AsNoTracking()
                .Where(p => p.Proponent != null)
                .GroupBy(p => p.ProjectType)
                .Select(g => new GroupedProjectTypesDTO
                {
                    Type = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return Ok(result);
        }
    }
}
