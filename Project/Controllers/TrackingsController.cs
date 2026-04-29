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
    public class TrackingsController : ControllerBase
    {
        private readonly DBContext _context;

        public TrackingsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Trackings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackingModel>>> GetTracking()
        {
            return await _context.Trackings.ToListAsync();
        }

        // GET: api/Trackings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrackingModel>> GetTrackingModel(Guid id)
        {
            var trackingModel = await _context.Trackings.FindAsync(id);

            if (trackingModel == null)
            {
                return NotFound();
            }

            return trackingModel;
        }

        // PUT: api/Trackings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrackingModel(Guid id, TrackingModel trackingModel)
        {
            if (id != trackingModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(trackingModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackingModelExists(id))
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

        // POST: api/Trackings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrackingModel>> PostTrackingModel(TrackingModel trackingModel)
        {
            _context.Trackings.Add(trackingModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrackingModel", new { id = trackingModel.Id }, trackingModel);
        }

        // DELETE: api/Trackings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrackingModel(Guid id)
        {
            var trackingModel = await _context.Trackings.FindAsync(id);
            if (trackingModel == null)
            {
                return NotFound();
            }

            _context.Trackings.Remove(trackingModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrackingModelExists(Guid id)
        {
            return _context.Trackings.Any(e => e.Id == id);
        }
    }
}
