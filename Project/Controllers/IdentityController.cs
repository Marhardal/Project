using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.DTO;
using Project.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public IdentityController(DBContext _dbContext, UserManager<IdentityUser> _userManager)
        {
            dbContext = _dbContext;
            userManager = _userManager;
        }

        // POST: api/Identity
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserModel>> CreateUser(IdentityDTO identity)
        {
            if (identity == null)
            {
                return NoContent();
            }

            var existingUser = await userManager.FindByEmailAsync(identity.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already exists.");
            }

            var existingUsername = await userManager.FindByNameAsync(identity.Username);
            if (existingUsername != null)
            {
                return BadRequest("Username already exists.");
            }

            var result = await userManager.CreateAsync(new IdentityUser
            {
                UserName = identity?.Username,
                Email = identity?.Email,
                PhoneNumber = identity?.Phone,
            }, identity.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
}
