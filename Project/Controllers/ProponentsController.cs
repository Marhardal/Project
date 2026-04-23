using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

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
            var proponent = await _context.Proponents.Where(p => p.ID == id)
                .Select
                (
                    p => new Proponent
                    {
                        ID = p.ID,
                        Name = p.Name,
                        Location = p.Location,
                        Address = p.Address,
                        createdOn = p.createdOn,
                        Projects = p.Projects == null ? null : p.Projects.Select(pro => new ProjectModel
                        {
                            Id = pro.Id,
                            Name = pro.Name,
                            Location = pro.Location,
                            createdOn = pro.createdOn,
                        }).ToList(),
                        Contacts = p.Contacts == null ? null : p.Contacts.Select(c => new ContactPerson
                        {
                            Id = c.Id,
                            FullName = c.FullName,
                            Email = c.Email,
                            Phone = c.Phone,
                            createdOn = c.createdOn,
                        }).ToList()
                    }
                ).FirstOrDefaultAsync();

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
