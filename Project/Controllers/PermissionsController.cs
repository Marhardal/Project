using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.DTO;
using System.Security.Claims;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly DBContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public PermissionsController(DBContext _context, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            context = _context;
            userManager = _userManager;
            roleManager = _roleManager;
        }
        [HttpGet("my-nav")]
        public async Task<IActionResult> GetMyPermissions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            
            

            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return Unauthorized();

            var roleNames = await userManager.GetRolesAsync(user);

            var roleIds = await context.Roles
                .Where(r => roleNames.Contains(r.Name!))
                .Select(r => r.Id)
                .ToListAsync();

            var permissions = await context.Permissions
                .Where(rp => roleIds.Contains(rp.RoleId) && rp.IsGranted)
                .Include(rp => rp.PageAction)
                    .ThenInclude(pa => pa.Page)
                .Select(rp => new PermissionDto
                {
                    PageName = rp.PageAction.Page.Name,
                    ActionName = rp.PageAction.Slug,
                    Icon = rp.PageAction.Page.Icon,
                    Slug = rp.PageAction.Page.Slug
                })
                .Distinct()
                .ToListAsync();

            return Ok(permissions);
        }
        }
    }

