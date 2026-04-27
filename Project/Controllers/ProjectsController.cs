using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.Models.ProjectManager.Data.Model;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly DBContext _context;

        public ProjectsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> GetProjects()
        {
            var projects = await _context.Projects
    .Include(p => p.Proponent)
    .Include(p => p.Trackings)
        .ThenInclude(t => t.Status)
    .Include(p => p.Trackings)
        .ThenInclude(t => t.Review)
    .AsNoTracking()
    .Select(p => new
    {
        p.Id,
        p.Location,
        p.Name,
        p.ProjectType,
        p.ProponentID,
        p.SubmissionDate,
        p.closingDate,
        p.assignedDate,
        p.Description,

        Proponent = p.Proponent == null ? null : new Proponent
        {
            ID = p.Proponent.ID,
            Name = p.Proponent.Name,
            Address = p.Proponent.Address,
            Location = p.Proponent.Location
        },

        Trackings = p.Trackings
            .OrderByDescending(t => t.createdOn)
            .Select(t => new TrackingModel
            {
                Id = t.Id,
                userID = t.userID,
                createdOn = t.createdOn,
                StatusID = t.StatusID,

                Status = t.Status == null ? null : new Status
                {
                    ID = t.Status.ID,
                    Name = t.Status.Name,
                    Color = t.Status.Color,
                    SortOrder = t.Status.SortOrder,
                },

                Review = t.Review == null ? null : new ReviewModel
                {
                    ID = t.Review.ID,
                    Remarks = t.Review.Remarks,
                    createdOn = t.Review.createdOn
                }
            })
            .ToList()
    })
    .ToListAsync();
            if (!projects.Any())
            {
                return NoContent();
            }
            return Ok(projects);
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectModel>> GetProjectModel(Guid id)
        {
            var projectModel = await _context.Projects
     .Where(p => p.Id == id)
     .Select(p => new ProjectModel
     {
         Id = p.Id,
         Name = p.Name,
         Location = p.Location,
         ProjectType = p.ProjectType,
         SubmissionDate = p.SubmissionDate,
         closingDate = p.closingDate,
         assignedDate = p.assignedDate,
         Description = p.Description,
         ProponentID = p.ProponentID,
         Proponent = p.Proponent == null ? null : new Proponent
         {
            ID = p.Proponent.ID,
            Name = p.Proponent.Name,
            Address = p.Proponent.Address,
            Location = p.Proponent.Location
         },

         Trackings = p.Trackings.Select(t => new TrackingModel
         {
             Id = t.Id,
             userID = t.userID,
             createdOn = t.createdOn,

             Status = t.Status == null ? null : new Status
             {
                 ID = t.Status.ID,
                 Name = t.Status.Name,
                 SortOrder = t.Status.SortOrder,
             },

             Review = t.Review == null ? null : new ReviewModel
             {
                 ID = t.Review.ID,
                 Remarks = t.Review.Remarks,
                 createdOn = t.Review.createdOn
             }
         }).ToList()
     })
     .FirstOrDefaultAsync();

            if (projectModel == null)
            {
                return NotFound();
            }

            return Ok(projectModel);
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectModel(Guid id, ProjectModel projectModel)
        {
            if (id != projectModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectModel).State = EntityState.Modified;
            TrackingModel tracking = new TrackingModel()
            {
                StatusID = projectModel.Tracking.StatusID,
                ProjectID = projectModel.Id,
                userID = Guid.Parse("97775807-c99a-445a-9bc3-2a88c3449823"),
                assignedDate = DateTime.UtcNow,
                createdOn = DateTime.UtcNow,
                updatedOn = DateTime.UtcNow,
            };
            _context.Tracking.Add(tracking);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectModel>> PostProjectModel(ProjectModel projectModel)
        {
            _context.Projects.Add(projectModel);
            var firstStatus = await _context.Statuses.OrderBy(s => s.SortOrder).FirstOrDefaultAsync();
            TrackingModel tracking = new TrackingModel()
            {
                StatusID = firstStatus.ID,
                    ProjectID = projectModel.Id,
                    userID = Guid.Parse("97775807-c99a-445a-9bc3-2a88c3449823"),
                    assignedDate = DateTime.UtcNow,
                createdOn = DateTime.UtcNow,
                updatedOn = DateTime.UtcNow,
            };
            _context.Tracking.Add(tracking);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProjectModel", new { id = projectModel.Id }, projectModel);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectModel(Guid id)
        {
            var projectModel = await _context.Projects.FindAsync(id);
            if (projectModel == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(projectModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectModelExists(Guid id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
