using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Project.Data;
using Project.Models;
using QuestPDF.Fluent;
using System.Drawing;
using System.Security.Claims;

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
           
            var projects = await _context.Projects
                .AsNoTracking()
                .Where(p => isProposal
        ? p.ProjectType == ProjectType.Brief
        : p.ProjectType != ProjectType.Brief)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.ProjectType,
                    p.ProponentID,
                    p.SubmissionDate,
                    p.closingDate,
                    p.assignedDate,
                    p.Description,
                    p.createdOn,
                    p.CategoryID,
                    Category = p.Category == null ? null : new CategoryModel
                    {
                        ID = p.Category.ID, 
                        Name = p.Category.Name
                    },
                    Proponent = p.Proponent == null ? null : new Proponent
                    {
                        ID = p.Proponent.ID,
                        Name = p.Proponent.Name,
                        Address = p.Proponent.Address,
                        Location = p.Proponent.Location,
                        createdOn = p.createdOn,
                    },
                    ProjectLocations = p.ProjectLocations
                        .Select(pl => new ProjectLocation
                        {
                            ID = pl.ID,
                            LocationID = pl.LocationID,
                            Location = pl.Location == null ? null : new LocationModel
                            {
                                Location = pl.Location.Location,
                                Code = pl.Location.Code
                            }
                        })
                        .ToList(),

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

                    ProjectLocations = p.ProjectLocations
                        .Select(pl => new
                        {
                            pl.ID,
                            Location = pl.Location == null ? null : new
                            {
                                pl.Location.ID,
                                pl.Location.Location
                            }
                        })
                        .ToList(),

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            if (id != projectModel.Id)
            {
                return BadRequest();
            }

            if (!projectModel.SelectedLocationIds.Any())
            {
                return StatusCode(422, new { StatusMessage = "Please add a Location." });
            }
            if (projectModel.SelectedLocationIds.Any())
            {
                foreach (var locationID in projectModel.SelectedLocationIds)
                {
                    _context.ProjectLocations.Where(pl => pl.ProjectID == projectModel.Id).ExecuteDelete();
                    _context.ProjectLocations.Add(new ProjectLocation
                    {
                        ProjectID = projectModel.Id,
                        LocationID = Guid.Parse(locationID)
                    });
                }
            }

            _context.Entry(projectModel).State = EntityState.Modified;
            TrackingModel tracking = new TrackingModel()
            {
                StatusID = projectModel.Tracking.StatusID,
                ProjectID = projectModel.Id,
                userID = userId,
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
        //[HttpPost]
        //public async Task<ActionResult<ProjectModel>> PostProjectModel(ProjectModel projectModel)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (userId is null)
        //        return Unauthorized();

        //    _context.Projects.Add(projectModel);
        //    ProjectLocation location = new ProjectLocation()
        //    {
        //        ProjectID = projectModel.Id,
        //        LocationID = projectModel.SelectedLocationIds
        //    };
        //    _context.ProjectLocations.Add(location);
        //    var firstStatus = await _context.Statuses.OrderBy(s => s.SortOrder).FirstOrDefaultAsync();
        //    TrackingModel tracking = new TrackingModel()
        //    {
        //        StatusID = firstStatus.ID,
        //        ProjectID = projectModel.Id,
        //        userID = userId,
        //        assignedDate = DateTime.UtcNow,
        //        createdOn = DateTime.UtcNow,
        //        updatedOn = DateTime.UtcNow,
        //    };
        //    _context.Trackings.Add(tracking);
        //    await _context.SaveChangesAsync();
        //    //return CreatedAtAction("GetProjectModel", new { id = projectModel.Id }, projectModel);
        //    return StatusCode(201, new { StatusMessage = "Added Contact Person." });
        //}
        [HttpPost]
        public async Task<ActionResult<ProjectModel>> PostProjectModel(ProjectModel projectModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            if (!projectModel.SelectedLocationIds.Any())
            {
                return StatusCode(422, new { StatusMessage = "Please add a Location." });
            }

            // Set timestamps
            projectModel.createdOn = DateTime.UtcNow;
            projectModel.updatedOn = DateTime.UtcNow;

            _context.Projects.Add(projectModel);

            // Add one ProjectLocation per selected location
            if (projectModel.SelectedLocationIds.Any())
            {
                foreach (var locationID in projectModel.SelectedLocationIds)
                {
                    _context.ProjectLocations.Add(new ProjectLocation
                    {
                        ProjectID = projectModel.Id,
                        LocationID = Guid.Parse(locationID)
                    });
                }
            }

            // Add initial tracking status
            var firstStatus = await _context.Statuses
                .OrderBy(s => s.SortOrder)
                .FirstOrDefaultAsync();

            if (firstStatus is null)
                return BadRequest("No statuses configured.");

            _context.Trackings.Add(new TrackingModel
            {
                StatusID = firstStatus.ID,
                ProjectID = projectModel.Id,
                userID = userId,
                assignedDate = DateTime.UtcNow,
                createdOn = DateTime.UtcNow,
                updatedOn = DateTime.UtcNow,
            });

            await _context.SaveChangesAsync();

            return StatusCode(201, new { StatusMessage = "Project added successfully." });
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

        [HttpGet("/api/export/projects/excel")]
        public async Task<IActionResult> ExportProjects(ProjectType? type = null, string filter = null, string statusID = null)
        {
            var query = _context.Projects
                .Where(p => type == null
                    ? p.ProjectType != ProjectType.Brief
                    : p.ProjectType == type)
                .Include(p => p.Proponent)
                .Include(p => p.Trackings)
                    .ThenInclude(t => t.Status)
                .AsQueryable();

            if (!string.IsNullOrEmpty(statusID) && Guid.TryParse(statusID, out var parsedStatusId))
            {
                query = query.Where(p => p.Trackings
                    .OrderByDescending(t => t.createdOn)
                    .Select(t => t.Status.ID)
                    .FirstOrDefault() == parsedStatusId);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(i => i.Name.Contains(filter));
            }

            var projects = await query.ToListAsync();

            if (!projects.Any())
            {
                return NotFound("No projects found.");
            }
            ExcelPackage.License.SetNonCommercialOrganization("MEPA");

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Projects");

            // Headers
            sheet.Cells[1, 1].Value = "#";
            sheet.Cells[1, 2].Value = "Project";
            sheet.Cells[1, 3].Value = "Proponent";
            sheet.Cells[1, 4].Value = "Type";
            sheet.Cells[1, 5].Value = "Status";
            sheet.Cells[1, 6].Value = "Submission Date";

            // Style headers
            using (var range = sheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // Data rows
            int row = 2;
            for (int i = 0; i < projects.Count; i++)
            {
                var project = projects[i];
                var latestStatus = project.Trackings
                    .OrderByDescending(t => t.createdOn)
                    .Select(t => t.Status?.Name)
                    .FirstOrDefault() ?? "No Status";

                sheet.Cells[row, 1].Value = i + 1;
                sheet.Cells[row, 2].Value = project.Name;
                sheet.Cells[row, 3].Value = project.Proponent?.Name;
                sheet.Cells[row, 4].Value = project.ProjectType.ToString();
                sheet.Cells[row, 5].Value = latestStatus;
                sheet.Cells[row, 6].Value = project.SubmissionDate.ToString("dd MMM yyyy");

                // Alternate row colors
                if (row % 2 == 0)
                {
                    using var range = sheet.Cells[row, 1, row, 6];
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                row++;
            }

            // Add totals row
            sheet.Cells[row, 1].Value = "Total";
            sheet.Cells[row, 2].Value = projects.Count;
            using (var totalRange = sheet.Cells[row, 1, row, 6])
            {
                totalRange.Style.Font.Bold = true;
                totalRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                totalRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            }

            // Auto fit columns
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

            // Add border to all cells
            using (var allCells = sheet.Cells[1, 1, row, 6])
            {
                allCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                allCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                allCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                allCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            var fileBytes = await package.GetAsByteArrayAsync();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Projects_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
            );
        }

        [HttpGet("/api/export/projects/pdf")]
        public async Task<IActionResult> ExportProjectsPDF(ProjectType? type = null, string? filter = null, string? statusID = null)
        {
            var query = _context.Projects
                .Where(p => type == null
                    ? p.ProjectType != ProjectType.Brief
                    : p.ProjectType == ProjectType.Brief)
                .Include(p => p.Proponent)
                .Include(p => p.Trackings)
                    .ThenInclude(t => t.Status)
                .AsQueryable();

            if (!string.IsNullOrEmpty(statusID) && Guid.TryParse(statusID, out var parsedStatusId))
            {
                query = query.Where(p => p.Trackings
                    .OrderByDescending(t => t.createdOn)
                    .Select(t => t.Status.ID)
                    .FirstOrDefault() == parsedStatusId);
            }

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(i => i.Name.Contains(filter));
            }

            var projects = await query.ToListAsync();

            if (!projects.Any())
            {
                return NotFound("No projects found.");
            }

            var stream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Projects Report").FontSize(18).Bold();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Name
                            columns.RelativeColumn(2); // Location
                            columns.RelativeColumn(2); // Type
                            columns.RelativeColumn(2); // Status
                        });

                        table.Header(header =>
                        {
                            header.Cell().Border(1).Padding(2).Text("Name").Bold();
                            header.Cell().Border(1).Padding(2).Text("Location").Bold();
                            header.Cell().Border(1).Padding(2).Text("Type").Bold();
                            header.Cell().Border(1).Padding(2).Text("Status").Bold();
                        });

                        foreach (var project in projects)
                        {
                            table.Cell().Border(1).Padding(2).Text(project.Name);
                            table.Cell().Border(1).Padding(2).Text(string.Join(", ", project.ProjectLocations.Select(pl => pl.Location.Location)));
                            table.Cell().Border(1).Padding(2).Text(project.ProjectType.ToString());
                            table.Cell().Border(1).Padding(2).Text(project.Trackings
        .OrderByDescending(t => t.createdOn)
        .FirstOrDefault()?.Status?.Name ?? "-");
                        }
                    });

                    page.Footer().AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                });
            })
            .GeneratePdf(stream);

            var fileBytes = stream.ToArray();

            return File(
                fileBytes,
                "application/pdf",
                $"Projects_{DateTime.Now:yyyyMMddHHmmss}.pdf"
            );
        }

        private bool ProjectModelExists(Guid id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
