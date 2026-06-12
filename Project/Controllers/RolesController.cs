using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.DTO;
using Project.Models;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly DBContext context;

        public RolesController(
            RoleManager<IdentityRole> _roleManager,
            UserManager<IdentityUser> _userManager,
            DBContext _db)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            context = _db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await roleManager.Roles.ToListAsync();

            var result = new List<RoleDTO>();
            foreach (var role in roles)
            {
                var perms = await context.Permissions
                    .Where(rp => rp.RoleId == role.Id && rp.IsGranted)
                    .Include(rp => rp.PageAction)
                        .ThenInclude(pa => pa.Page)
                    .ToListAsync();

                result.Add(new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name!,
                    Permissions = perms.Select(rp => new PermissionDto
                    {
                        PageActionId = rp.PageActionId,
                        PageName = rp.PageAction.Page.Name,
                        ActionName = rp.PageAction.Name
                    }).ToList()
                });
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                return BadRequest("Role already exists.");

            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();

            var result = await roleManager.DeleteAsync(role);
            return result.Succeeded ? NoContent() : BadRequest(result.Errors);
        }

        // Bulk-save the entire permission matrix for a role
        [HttpPut("{roleId}/permissions")]
        public async Task<IActionResult> SavePermissions(
            string roleId, [FromBody] List<SavePermissionDto> permissions)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();

            // Remove existing permissions for this role and replace
            var existing = context.Permissions.Where(rp => rp.RoleId == roleId);
            context.Permissions.RemoveRange(existing);

            context.Permissions.AddRange(permissions.Select(p => new PermissionModel
            {
                RoleId = roleId,
                PageActionId = p.PageActionId,
                IsGranted = p.IsGranted
            }));

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
