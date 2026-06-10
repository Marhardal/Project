using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Migrations;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PageActions = Project.Models.PageActions;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageActionsController : ControllerBase
    {
        private readonly DBContext _context;

        public PageActionsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/PageActions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PageActions>>> GetPageActions()
        {
            var items = await _context.PageActions
                .AsNoTracking()
                .Include(pa => pa.Page)
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/PageActions/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PageActions>> GetPageActions(Guid id)
        {
            var item = await _context.PageActions
                .AsNoTracking()
                .Include(pa => pa.Page)
                .FirstOrDefaultAsync(pa => pa.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/PageActions/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutPageActions(Guid id, PageActions pageActions)
        {
            if (id != pageActions.Id)
            {
                return BadRequest();
            }

            _context.Entry(pageActions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageActionsExists(id))
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

        // POST: api/PageActions
        [HttpPost("{pageId}/actions")]
        public async Task<ActionResult> PostPageActions(Guid pageId, Models.PageActions pageActions)
        {
            //pageActions.Id = Guid.NewGuid();
            //_context.PageActions.Add(pageActions);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetPageActions), new { id = pageActions.Id }, pageActions);
            var page = await _context.Pages.FindAsync(pageId);
            if (page is null) return NotFound();

            var action = new PageActions
            {
                Id = Guid.NewGuid(),
                PageID = pageId,
                Name = pageActions.Name,
                Slug = pageActions.Name.ToLower().Replace(" ", "-")
            };

            _context.PageActions.Add(action);
            await _context.SaveChangesAsync();
            return Ok(action.Id);
        }

        // DELETE: api/PageActions/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePageActions(Guid id)
        {
            var item = await _context.PageActions.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.PageActions.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PageActionsExists(Guid id)
        {
            return _context.PageActions.Any(e => e.Id == id);
        }
    }
}
