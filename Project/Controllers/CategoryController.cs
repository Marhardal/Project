using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly DBContext _context;
    public CategoryController(DBContext context)
    {
        _context = context;
    }

    // GET: api/Category
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategoryModel()
    {
        return await _context.Categories.ToListAsync();
    }

    // GET: api/Category/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryModel>> GetCategoryModel(System.Guid id)
    {
        var categorymodel = await _context.Categories.FindAsync(id);

        if (categorymodel == null)
        {
            return NotFound();
        }

        return categorymodel;
    }

    // PUT: api/Category/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategoryModel(System.Guid? id, CategoryModel categorymodel)
    {
        if (id != categorymodel.ID)
        {
            return BadRequest();
        }

        _context.Entry(categorymodel).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryModelExists(id))
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

    // POST: api/Category
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<CategoryModel>> PostCategoryModel(CategoryModel categorymodel)
    {
        _context.Categories.Add(categorymodel);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCategoryModel", new { id = categorymodel.ID }, categorymodel);
    }

    // DELETE: api/Category/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoryModel(System.Guid? id)
    {
        var categorymodel = await _context.Categories.FindAsync(id);
        if (categorymodel == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(categorymodel);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoryModelExists(System.Guid? id)
    {
        return _context.Categories.Any(e => e.ID == id);
    }
}
