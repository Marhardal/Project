using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.DTO;
using Project.Migrations;
using Project.Models;

namespace Project.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DBContext _context;

        public HomeController(DBContext context)
        {
            _context = context;
        }

        [HttpGet("api/status-summary")]
        public async Task<ActionResult<IEnumerable<StatusSummaryDTO>>> Index(DateTime? From, DateTime? To)
        {
            var fromDate = From ?? DateTime.Now.AddDays(-7);
            var toDate = To ?? DateTime.Now;

            var result = await _context.Projects
                .Where(p => p.SubmissionDate >= fromDate && p.SubmissionDate <= toDate)
                .Select(p => new
                {
                    Type = p.ProjectType,
                    Name = p.Trackings
                        .OrderByDescending(t => t.createdOn)
                        .Select(t => t.Status!.Name)
                        .FirstOrDefault(),
                    Color = p.Trackings
                        .OrderByDescending(t => t.createdOn)
                        .Select(t => t.Status!.Color)
                        .FirstOrDefault()
                })
                .GroupBy(x => new
                {
                    x.Type,
                    Name = x.Name ?? "No Status"
                })
                .Select(g => new StatusSummaryDTO
                {
                    Type = g.Key.Type,
                    Name = g.Key.Name,
                    Total = g.Count(),
                    Color = g.Select(x => x.Color).FirstOrDefault()
                })
                .OrderBy(x => x.Type)
                .ThenBy(x => x.Name)
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/Proponents
        [HttpGet("api/GetProjectbyProponent")]
        public async Task<ActionResult<IEnumerable<ProjectbyProponentDTO>>> GetProjectbyProponent(DateTime? From, DateTime? To)
        {
            var fromDate = From ?? DateTime.Now.AddDays(-7);
            var toDate = To ?? DateTime.Now;
            return await _context.Projects.Where(p => p.Proponent != null && 
            p.SubmissionDate >= fromDate && p.SubmissionDate <= toDate).GroupBy(p => p.Proponent.Name).OrderByDescending(p => p.Count())
            .Select(g => new ProjectbyProponentDTO
            {
                Proponent = g.Key!,
                Count = g.Count()
            }).Take(5).ToListAsync();
        }

        [HttpGet("api/GetRecentProjects")]
        public async Task<ActionResult<IEnumerable<RecentActivityDTO>>> GetRecentProject(DateTime? From, DateTime? To)
        {
            var fromDate = From ?? DateTime.Now.AddDays(-7);
            var toDate = To ?? DateTime.Now;
            var trackings = await _context.Trackings
                .Include(t => t.Project)
                    .ThenInclude(p => p.Proponent)
                .Include(t => t.Status)
                .OrderByDescending(t => t.createdOn)
                .Take(50)
                .ToListAsync();

            var result = trackings.Where(p => p.Project.SubmissionDate >= fromDate && p.Project.SubmissionDate <= toDate)
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
        public async Task<ActionResult<IEnumerable<GroupedProjectTypesDTO>>> GetGroupedProjects(DateTime? From, DateTime? To)
        {
            var fromDate = From ?? DateTime.Now.AddDays(-7);
            var toDate = To ?? DateTime.Now;
            var result = await _context.Projects
                .AsNoTracking()
                .Where(p => p.Proponent != null && p.SubmissionDate >= fromDate && p.SubmissionDate <= toDate)
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

        [HttpGet("api/GetProjectStatus")]
        public async Task<ActionResult<IEnumerable<ProjectStatus>>> GetProjectStatus(DateTime? From, DateTime? To)
        {
            var fromDate = From ?? DateTime.Now.AddDays(-7);
            var toDate = To ?? DateTime.Now;

            var latestStatuses = await _context.Trackings
                .Where(t => t.Project.SubmissionDate >= fromDate &&
                            t.Project.SubmissionDate <= toDate)
                .GroupBy(t => t.ProjectID)
                .Select(g => g
                    .OrderByDescending(t => t.createdOn)
                    .Select(t => new
                    {
                        t.ProjectID,
                        StatusName = t.Status.Name,
                        // join Status inside Select
                        t.Status.Color
                    })
                    .First())
                .ToListAsync();
            var result = latestStatuses
    .GroupBy(t => t.StatusName ?? "No Status")
    .Select(g => new ProjectStatus
    {
        Status = g.Key,
        Total = g.Count(),
        Color = g.First().Color
    })
    .OrderByDescending(x => x.Total)
    .ToList();

            return Ok(result);
        }

        [HttpGet("api/GetProjectCategories")]
        public async Task<ActionResult<IEnumerable<ProjectStatus>>> GetProjectCategories(DateTime? From, DateTime? To)
        {
            var fromDate = From ?? DateTime.Now.AddDays(-7);
            var toDate = To ?? DateTime.Now;
            var result = await _context.Projects
                .Where(p => p.SubmissionDate >= fromDate &&
                            p.SubmissionDate <= toDate &&
                            p.ProjectType != ProjectType.Brief)
                .Include(p => p.Category)
                .AsNoTracking()
                .GroupBy(p => new { p.CategoryID, p.Category.Name })
                .Select(g => new ProjectCategory
                {
                    Category = g.Key.Name,
                    Total = g.Count()
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("api/projects-by-month")]
        public async Task<ActionResult<IEnumerable<ProjectMonthDTO>>> GetProjectsByMonth(DateTime? From, DateTime? To)
        {
            var startDate = DateTime.UtcNow.AddMonths(-11);
            var fromDate = From ?? DateTime.Now.AddDays(-7);
            var toDate = To ?? DateTime.Now;
            var result = await _context.Projects
                .AsNoTracking()
                .Where(p => p.SubmissionDate >= fromDate && p.SubmissionDate <= toDate
                            && p.ProjectType != ProjectType.Brief)
                .GroupBy(p => new
                {
                    p.SubmissionDate.Year,
                    p.SubmissionDate.Month,
                    p.ProjectType
                })
                .Select(g => new ProjectMonthDTO
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM"),
                    Type = g.Key.ProjectType,
                    Total = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ThenBy(x => x.Type)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("api/projects-by-District")]
        public async Task<ActionResult<IEnumerable<DistrictDistribution>>> GetProjectsByDistrict(DateTime? From, DateTime? To)
        {
            var fromDate = From ?? DateTime.Now.AddDays(-7);
            var toDate = To ?? DateTime.Now;
            var result = await _context.ProjectLocations
    .AsNoTracking()
    .Where(pl => pl.Project.SubmissionDate >= fromDate &&
                 pl.Project.SubmissionDate <= toDate &&
                 pl.Project.ProjectType != ProjectType.Brief)
    .GroupBy(pl => new
    {
        pl.LocationID,
        pl.Location.Location,
        pl.Location.Code
    })
    .Select(g => new DistrictDistribution
    {
        District = g.Key.Location,
        Code = g.Key.Code,
        Total = g.Select(x => x.ProjectID).Distinct().Count(),

        Project = g.GroupBy(x => x.Project.ProjectType)
            .Select(pt => new ProjectMonthDTO
            {
                Type = pt.Key,
                Total = pt.Select(x => x.ProjectID).Distinct().Count()
            })
            .ToList()
    })
    .OrderByDescending(x => x.Total)
    .ToListAsync();

            return Ok(result);
        }
    }
}
