using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{
    private readonly DBContext _context;
    public LocationsController(DBContext context)
    {
        _context = context;
    }

    // GET: api/Locations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationModel>>> GetLocation()
    {
        return await _context.Locations.ToListAsync();
    }

    // GET: api/Locations/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LocationModel>> GetLocation(System.Guid id)
    {
        var locationmodel = await _context.Locations.FindAsync(id);

        if (locationmodel == null)
        {
            return NotFound();
        }

        return locationmodel;
    }

    // PUT: api/Location/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLocation(System.Guid? id, LocationModel locationmodel)
    {
        if (id != locationmodel.ID)
        {
            return BadRequest();
        }

        _context.Entry(locationmodel).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LocationModelExists(id))
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

    // POST: api/Location
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<LocationModel>> PostLocation(LocationModel locationmodel)
    {
        _context.Locations.Add(locationmodel);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetLocationModel", new { id = locationmodel.ID }, locationmodel);
    }

    // DELETE: api/Location/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(System.Guid? id)
    {
        var locationmodel = await _context.Locations.FindAsync(id);
        if (locationmodel == null)
        {
            return NotFound();
        }

        _context.Locations.Remove(locationmodel);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LocationModelExists(System.Guid? id)
    {
        return _context.Locations.Any(e => e.ID == id);
    }
}
