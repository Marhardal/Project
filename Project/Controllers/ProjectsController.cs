using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

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
        //[HttpGet("{isProposal}")]
        //public async Task<ActionResult<IEnumerable<ProjectModel>>> GetProjects(bool isProposal)
        //{
        //    int SortOrder;

        //    var approvedStatus = await _context.Statuses.ToListAsync();

        //    if (approvedStatus == null)
        //    {
        //        return NoContent();
        //    }

        //    SortOrder = isProposal ? 3 : approvedStatus.Count();

        //    var projects = await _context.Projects
        //        .AsNoTracking()
        //        .Where(p => p.Trackings.LastOrDefault().Status.SortOrder == SortOrder)
        //        .Select(p => new
        //        {
        //            p.Id,
        //            p.Location,
        //            p.Name,
        //            p.ProjectType,
        //            p.ProponentID,
        //            p.SubmissionDate,
        //            p.closingDate,
        //            p.assignedDate,
        //            p.Description,

        //            Proponent = p.Proponent == null ? null : new Proponent
        //            {
        //                ID = p.Proponent.ID,
        //                Name = p.Proponent.Name,
        //                Address = p.Proponent.Address,
        //                Location = p.Proponent.Location
        //            },

        //            Trackings = p.Trackings
        //                .OrderByDescending(t => t.createdOn)
        //                .Select(t => new TrackingModel
        //                {
        //                    Id = t.Id,
        //                    userID = t.userID,
        //                    createdOn = t.createdOn,
        //                    StatusID = t.StatusID,

        //                    Status = t.Status == null ? null : new Status
        //                    {
        //                        ID = t.Status.ID,
        //                        Name = t.Status.Name,
        //                        Color = t.Status.Color,
        //                        SortOrder = t.Status.SortOrder
        //                    },

        //                    Review = t.Review == null ? null : new ReviewModel
        //                    {
        //                        ID = t.Review.ID,
        //                        Remarks = t.Review.Remarks,
        //                        createdOn = t.Review.createdOn
        //                    }
        //                })
        //                .ToList()
        //        })
        //        .ToListAsync();
        //    if (!projects.Any())
        //    {
        //        return NoContent();
        //    }
        //    return Ok(projects);
        //}

        [HttpGet("filter/{isProposal:bool}")]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> GetProjects(bool isProposal = true)
        {
            var maxSortOrder = await _context.Statuses
                .MaxAsync(s => s.SortOrder);

            var targetSortOrder = isProposal ? 3 : maxSortOrder;

            var projects = await _context.Projects
                .AsNoTracking()
                .Where(p => p.Trackings
                    .OrderByDescending(t => t.createdOn)
                    .Select(t => t.Status.SortOrder)
                    .FirstOrDefault() < targetSortOrder)
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
                    p.createdOn,

                    Proponent = p.Proponent == null ? null : new Proponent
                    {
                        ID = p.Proponent.ID,
                        Name = p.Proponent.Name,
                        Address = p.Proponent.Address,
                        Location = p.Proponent.Location,
                        createdOn = p.createdOn,
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
                                SortOrder = t.Status.SortOrder
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
                return NoContent();

            return Ok(projects);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetProjectModel(Guid id)
        {
            var projectModel = await _context.Projects
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Location,
                    p.ProjectType,
                    p.SubmissionDate,
                    p.closingDate,
                    p.assignedDate,
                    p.Description,
                    p.ProponentID,
                    p.createdOn,
                    Proponent = p.Proponent == null ? null : new
                    {
                        p.Proponent.ID,
                        p.Proponent.Name,
                        p.Proponent.Address,
                        p.Proponent.Location
                    },

                    Trackings = p.Trackings
                        .OrderByDescending(t => t.createdOn)
                        .Select(t => new
                        {
                            t.Id,
                            t.userID,
                            t.createdOn,

                            Status = t.Status == null ? null : new
                            {
                                t.Status.ID,
                                t.Status.Name,
                                t.Status.SortOrder
                            },

                            Review = t.Review == null ? null : new
                            {
                                t.Review.ID,
                                t.Review.Remarks,
                                t.Review.createdOn
                            },

                            User = t.User == null ? null : new
                            {
                                t.User.FirstName,
                                t.User.Surname
                            }
                        })
                        .ToList()
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
                userID = "94f18329-f144-41bd-8f25-22b887f686e7",
                assignedDate = DateTime.UtcNow,
                createdOn = DateTime.UtcNow,
                updatedOn = DateTime.UtcNow,
            };
            _context.Trackings.Add(tracking);
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
                userID = "94f18329-f144-41bd-8f25-22b887f686e7",
                assignedDate = DateTime.UtcNow,
                createdOn = DateTime.UtcNow,
                updatedOn = DateTime.UtcNow,
            };
            _context.Trackings.Add(tracking);
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
