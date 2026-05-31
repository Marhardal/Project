using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Project.Data;
using Project.Models;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProponentsController : ControllerBase
    {
        private readonly DBContext _context;

        public ProponentsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Proponents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proponent>>> GetProponents()
        {
            return await _context.Proponents.ToListAsync();
        }

        // GET: api/Proponents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proponent>> GetProponent(Guid id)
        {
            var proponent = await _context.Proponents
    .AsNoTracking()
    .Where(p => p.ID == id)
    .Select(p => new Proponent
    {
        ID = p.ID,
        Name = p.Name,
        Location = p.Location,
        Address = p.Address,
        createdOn = p.createdOn,

        Projects = p.Projects.Select(pro => new ProjectModel
        {
            Id = pro.Id,
            Name = pro.Name,
            Location = pro.Location,
            createdOn = pro.createdOn,
        }).ToList(),

        Contacts = p.Contacts.Select(c => new ContactPerson
        {
            Id = c.Id,
            FullName = c.FullName,
            Email = c.Email,
            Phone = c.Phone,
            createdOn = c.createdOn,
        }).ToList()
    })
    .FirstOrDefaultAsync();
            if (proponent == null)
            {
                return NotFound();
            }

            return proponent;
        }

        // PUT: api/Proponents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProponent(Guid id, Proponent proponent)
        {
            if (id != proponent.ID)
            {
                return BadRequest();
            }

            _context.Entry(proponent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProponentExists(id))
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

        // POST: api/Proponents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Proponent>> PostProponent(Proponent proponent)
        {
            _context.Proponents.Add(proponent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProponent", new { id = proponent.ID }, proponent);
        }

        [HttpGet("/api/export/proponents/excel")]
        public async Task<IActionResult> ExportProponents(string filter = null, bool proposal = true)
        {
            var query = _context.Proponents.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(i => i.Name.Contains(filter) || i.Location.Contains(filter) || i.Address.Contains(filter));
            }

            var proponents = await query.ToListAsync();

            if (!proponents.Any())
            {
                return NotFound("No projects found.");
            }

            ExcelPackage.License.SetNonCommercialOrganization("MEPA");

            using var package = new ExcelPackage();
            var sheet = package.Workbook.Worksheets.Add("Projects");

            // Headers
            sheet.Cells[1, 1].Value = "#";
            sheet.Cells[1, 2].Value = "Name";
            sheet.Cells[1, 3].Value = "Postal Address";
            sheet.Cells[1, 4].Value = "Location";
            //sheet.Cells[1, 5].Value = "Status";
            //sheet.Cells[1, 6].Value = "Submission Date";

            // Style headers
            using (var range = sheet.Cells[1, 1, 1, 4])
            {
                range.Style.Font.Bold = true;
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // Data rows
            int row = 2;
            for (int i = 0; i < proponents.Count; i++)
            {
                var proponent = proponents[i];

                sheet.Cells[row, 1].Value = i + 1;
                sheet.Cells[row, 2].Value = proponent.Name;
                sheet.Cells[row, 3].Value = proponent.Address;
                sheet.Cells[row, 4].Value = proponent.Location;
                //sheet.Cells[row, 5].Value = proponent.;
                //sheet.Cells[row, 6].Value = project.SubmissionDate.ToString("dd MMM yyyy");

                // Alternate row colors
                if (row % 2 == 0)
                {
                    using var range = sheet.Cells[row, 1, row, 4];
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                row++;
            }

            // Add totals row
            //sheet.Cells[row, 1].Value = "Total";
            //sheet.Cells[row, 2].Value = projects.Count;
            //using (var totalRange = sheet.Cells[row, 1, row, 6])
            //{
            //    totalRange.Style.Font.Bold = true;
            //    totalRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            //    totalRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            //}

            // Auto fit columns
            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

            // Add border to all cells
            using (var allCells = sheet.Cells[1, 1, row, 4])
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
                $"Proponents_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
            );
        }

        [HttpGet("/api/export/proponents/pdf")]
        public async Task<IActionResult> ExportProponentsPDF(string filter = null, bool proposal = true)
        {
            var query = _context.Proponents.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(i => i.Name.Contains(filter) || i.Location.Contains(filter) || i.Address.Contains(filter));
            }

            var proponents = await query.ToListAsync();

            if (!proponents.Any())
            {
                return NotFound("No proponents found.");
            }

            var stream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Proponents List").FontSize(18).Bold();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Name
                            columns.RelativeColumn(2); // Location
                            columns.RelativeColumn(2); // Type
                            //columns.RelativeColumn(2); // Status
                        });

                        table.Header(header =>
                        {
                            header.Cell().Border(1).Padding(2).Text("Name").Bold();
                            header.Cell().Border(1).Padding(2).Text("Location").Bold();
                            header.Cell().Border(1).Padding(2).Text("Address").Bold();
                            //header.Cell().Border(1).Padding(2).Text("Status").Bold();
                        });

                        foreach (var proponent in proponents)
                        {
                            table.Cell().Border(1).Padding(2).Text(proponent.Name);
                            table.Cell().Border(1).Padding(2).Text(proponent.Location);
                            table.Cell().Border(1).Padding(2).Text(proponent.Address);
        //                    table.Cell().Border(1).Padding(2).Text(project.Trackings
        //.OrderByDescending(t => t.createdOn)
        //.FirstOrDefault()?.Status?.Name ?? "-");
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
                $"Proponents_{DateTime.Now:yyyyMMddHHmmss}.pdf"
            );
        }

        // DELETE: api/Proponents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProponent(Guid id)
        {
            var proponent = await _context.Proponents.FindAsync(id);
            if (proponent == null)
            {
                return NotFound();
            }

            _context.Proponents.Remove(proponent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProponentExists(Guid id)
        {
            return _context.Proponents.Any(e => e.ID == id);
        }
    }
}
