using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Migrations;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PagesController : ControllerBase
    {
        private readonly DBContext _context;

        public PagesController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Pages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagesModel>>> GetPages()
        {
            //return await _context.Pages
            //    .AsNoTracking()
            //    .ToListAsync();
            var pages = await _context.Pages
            .Include(p => p.PageActions)
            .OrderBy(p => p.SortOrder)
            .Select(p => new PagesModel
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Icon = p.Icon,
                SortOrder = p.SortOrder,
                PageActions = p.PageActions.Select(a => new Models.PageActions
                {
                    Id = a.Id,
                    Name = a.Name,
                    Slug = a.Slug
                }).ToList()
            })
            .ToListAsync();

            return Ok(pages);
        }

        // GET: api/Pages/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PagesModel>> GetPagesModel(Guid id)
        {
            var pagesModel = await _context.Pages
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pagesModel == null)
            {
                return NotFound();
            }

            return pagesModel;
        }

        // PUT: api/Pages/{id}
        [HttpPut("{id:guid}")]
        //    public async Task<IActionResult> PutPagesModel(Guid id, PagesModel pagesModel)
        //    {
        //        if (id != pagesModel.Id)
        //        {
        //            return BadRequest();
        //        }

        //        //_context.PageActions.Where(a => a.PageID == pagesModel.Id).ExecuteDelete();

        //        //var page = new PagesModel
        //        //{
        //        //    Name = pagesModel.Name,
        //        //    Slug = pagesModel.Name.ToLower().Replace(" ", "-"),
        //        //    Icon = pagesModel.Icon,
        //        //    SortOrder = pagesModel.SortOrder,
        //        //    PageActions = pagesModel.PageActions.Select(a => new Models.PageActions
        //        //    {
        //        //        Id = Guid.NewGuid(),
        //        //        Name = a.Name,
        //        //        Slug = a.Slug.ToLower().Replace(" ", "-")
        //        //    }).ToList()
        //        //};

        //        //_context.Entry(page).State = EntityState.Modified;

        //        var existing = await _context.Pages
        //.Include(p => p.PageActions)
        //.FirstOrDefaultAsync(p => p.Id == id);

        //        if (existing == null) return NotFound();

        //        existing.Name = pagesModel.Name;
        //        existing.Slug = pagesModel.Name.ToLower().Replace(" ", "-");
        //        existing.Icon = pagesModel.Icon;
        //        existing.SortOrder = pagesModel.SortOrder;

        //        // replace child actions
        //        _context.PageActions.RemoveRange(existing.PageActions);
        //        existing.PageActions = pagesModel.PageActions.Select(a => new Models.PageActions
        //        {
        //            Id = Guid.NewGuid(),
        //            PageID = id,
        //            Name = a.Name,
        //            Slug = a.Slug?.ToLower().Replace(" ", "-")
        //        }).ToList();

        //        _context.Entry(existing).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PagesModelExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return NoContent();
        //    }
        //public async Task<IActionResult> PutPagesModel(Guid id, PagesModel pagesModel)
        //{
        //    if (id != pagesModel.Id)
        //        return BadRequest();

        //    var existing = await _context.Pages
        //        .Include(p => p.PageActions)
        //        .FirstOrDefaultAsync(p => p.Id == id);

        //    if (existing is null) return NotFound();

        //    // Step 1 — delete old actions and commit immediately
        //    if (existing.PageActions.Any())
        //    {
        //        _context.PageActions.RemoveRange(existing.PageActions);
        //        await _context.SaveChangesAsync();
        //        existing.PageActions.Clear();
        //    }

        //    // Step 2 — update parent and add new actions
        //    existing.Name = pagesModel.Name.Trim();
        //    existing.Slug = pagesModel.Name.Trim().ToLower().Replace(" ", "-");
        //    existing.Icon = pagesModel.Icon;
        //    existing.SortOrder = pagesModel.SortOrder;

        //    foreach (var a in pagesModel.PageActions)
        //    {
        //        existing.PageActions.Add(new Models.PageActions
        //        {
        //            Id = Guid.NewGuid(),
        //            PageID = id,
        //            Name = a.Name.Trim(),
        //            Slug = a.Name.Trim().ToLower().Replace(" ", "-")
        //        });
        //    }

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        public async Task<IActionResult> PutPagesModel(Guid id, PagesModel pagesModel)
        {
            if (id != pagesModel.Id)
                return BadRequest();

            // Step 1 — delete old actions and commit
            var oldActions = await _context.PageActions
                .Where(a => a.PageID == id)
                .ToListAsync();

            if (oldActions.Any())
            {
                _context.PageActions.RemoveRange(oldActions);
                await _context.SaveChangesAsync();
            }

            // Step 2 — fetch the parent fresh (no stale tracking state)
            var existing = await _context.Pages
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existing is null) return NotFound();

            // Step 3 — update parent scalar properties
            existing.Name = pagesModel.Name.Trim();
            existing.Slug = pagesModel.Slug.Trim().ToLower().Replace(" ", "-");
            existing.Icon = pagesModel.Icon;
            existing.SortOrder = pagesModel.SortOrder;

            // Step 4 — add new actions
            var newActions = pagesModel.PageActions.Select(a => new Models.PageActions
            {
                Id = Guid.NewGuid(),
                PageID = id,
                Name = a.Name.Trim(),
                Slug = a.Name.Trim().ToLower().Replace(" ", "-")
            }).ToList();

            await _context.PageActions.AddRangeAsync(newActions);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // POST: api/Pages
        [HttpPost]
        public async Task<ActionResult<PagesModel>> PostPagesModel(PagesModel pagesModel)
        {
            //_context.Pages.Add(pagesModel);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetPagesModel", new { id = pagesModel.Id }, pagesModel);
            var page = new PagesModel
            {
                Id = Guid.NewGuid(),
                Name = pagesModel.Name,
                Slug = pagesModel.Name.ToLower().Replace(" ", "-"),
                Icon = pagesModel.Icon,
                SortOrder = pagesModel.SortOrder,
                PageActions = pagesModel.PageActions.Select(a => new Models.PageActions
                {
                    Id = Guid.NewGuid(),
                    Name = a.Name,
                    Slug = a.Slug.ToLower().Replace(" ", "-")
                }).ToList()
            };

            _context.Pages.Add(page);
            await _context.SaveChangesAsync();
            return Ok(page.Id);
        }

        // DELETE: api/Pages/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePagesModel(Guid id)
        {
            //var pagesModel = await _context.Pages.FindAsync(id);
            //if (pagesModel == null)
            //{
            //    return NotFound();
            //}

            //_context.Pages.Remove(pagesModel);
            //await _context.SaveChangesAsync();

            //return NoContent();
            var page = await _context.Pages.FindAsync(id);
            if (page is null) return NotFound();
            _context.Pages.Remove(page);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PagesModelExists(Guid id)
        {
            return _context.Pages.Any(e => e.Id == id);
        }
    }
}
