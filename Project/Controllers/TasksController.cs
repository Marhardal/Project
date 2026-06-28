using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly DBContext _context;
    public TasksController(DBContext context)
    {
        _context = context;
    }

    // GET: api/TasksModel
    [HttpGet("{ProjectID}")]
    public async Task<ActionResult<IEnumerable<TasksModel>>> GetTasksModel(Guid ProjectID)
    {
        var tasks = await _context.Tasks.Where(t => t.ProjectID == ProjectID).AsNoTracking().
            Select(t => new TasksModel
            {
                ID = t.ID,
                Name = t.Name,
                StatusID = t.StatusID,
                DueDate = t.DueDate,
                Priority = t.Priority,
                Description = t.Description,
                Status = t.Status == null ? null : new Status
                {
                    ID = t.ID,
                    Name = t.Status.Name
                },
                TaskAssignees = t.TaskAssignees.Select(ta => new TaskAssigneesModel
                {
                    ID = ta.ID,
                    User = ta.User == null ? null : new UserModel
                    {
                        FirstName =ta.User.FirstName,
                        Surname = ta.User.Surname
                    }
                    
                }).ToList()
            })
            .ToListAsync();

        if (!tasks.Any())
        {
            return NoContent();
        }

        return tasks;
    }

    // GET: api/TasksModel/5
    [HttpGet("{projectId}/{id}")]
    public async Task<ActionResult<TasksModel>> GetTasksModel(Guid projectId, Guid id)
    {
        var tasksmodel = await _context.Tasks.FindAsync(id);

        if (tasksmodel == null)
        {
            return NotFound();
        }

        return tasksmodel;
    }

    // PUT: api/TasksModel/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTasksModel(System.Guid? id, TasksModel tasksmodel)
    {
        if (id != tasksmodel.ID)
        {
            return BadRequest();
        }

        _context.Entry(tasksmodel).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TasksModelExists(id))
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

    // POST: api/TasksModel
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TasksModel>> PostTasksModel(TasksModel tasksmodel)
    {
        _context.Tasks.Add(tasksmodel);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTasksModel", new { id = tasksmodel.ID }, tasksmodel);
    }

    // DELETE: api/TasksModel/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTasksModel(System.Guid? id)
    {
        var tasksmodel = await _context.Tasks.FindAsync(id);
        if (tasksmodel == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(tasksmodel);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TasksModelExists(System.Guid? id)
    {
        return _context.Tasks.Any(e => e.ID == id);
    }
}
