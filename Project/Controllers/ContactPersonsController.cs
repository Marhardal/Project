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
    public class ContactPersonsController : ControllerBase
    {
        private readonly DBContext _context;

        public ContactPersonsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/ContactPersons
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ContactPerson>>> GetContacts(Guid id)
        {
            return await _context.Contacts.Where(c => c.ProponentID == id).ToListAsync();
        }

        // GET: api/ContactPersons/5
        [HttpGet("{Proponentid}/{id}")]
        public async Task<ActionResult<ContactPerson>> GetContactPerson(Guid id)
        {
            var contactPerson = await _context.Contacts.FindAsync(id);

            if (contactPerson == null)
            {
                return NotFound();
            }

            return contactPerson;
        }

        // PUT: api/ContactPersons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactPerson(Guid id, ContactPerson contactPerson)
        {
            if (id != contactPerson.Id)
            {
                return BadRequest();
            }

            _context.Entry(contactPerson).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactPersonExists(id))
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

        // POST: api/ContactPersons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ContactPerson>> PostContactPerson(ContactPerson contactPerson)
        {
            _context.Contacts.Add(contactPerson);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactPerson", new { id = contactPerson.Id }, contactPerson);
        }

        // DELETE: api/ContactPersons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactPerson(Guid id)
        {
            var contactPerson = await _context.Contacts.FindAsync(id);
            if (contactPerson == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contactPerson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactPersonExists(Guid id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
