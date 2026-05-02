using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.DTO;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        public UsersController(DBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDTO>>> GetUserProfils()
        {
            var userProfiles = await _context.UserProfiles.Include(i => i.identityUser)
                .Select(
                    u => new
                    {
                        u.UserID,
                        u.FirstName,
                        u.Surname,
                        u.DOB,
                        u.Gender,
                        u.Title,
                        u.createdOn,
                        IdentityDTO = u.identityUser == null ? null : new IdentityDTO
                        {
                            Email = u.identityUser.Email,
                            Username = u.identityUser.UserName,
                            Phone = u.identityUser.PhoneNumber,
                            TwoFactorEnabled = u.identityUser.TwoFactorEnabled,
                            AccessFailedCount = u.identityUser.AccessFailedCount,
                           LockoutEnabled = u.identityUser.LockoutEnabled
                        }
                    }
                )
                .ToListAsync();
            if (userProfiles is null || !userProfiles.Any())
            {
                return NoContent();
            }
            return Ok(userProfiles);
        }

        // GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<UserModel>> GetUserModel(Guid id)
        //{
        //    var userModel = await _context.UserProfils.FindAsync(id);

        //    if (userModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return userModel;
        //}

        // GET: api/Users/5
        [HttpGet("{userID}")]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfile(Guid userID)
        {
            var userModel = await _context.UserProfiles.Where(u => u.UserID == userID.ToString())
                .Include(i => i.identityUser)
                .Select(
                    u => new
                    {
                        u.ID,
                        u.UserID,
                        u.FirstName,
                        u.Surname,
                        u.DOB,
                        u.Gender,
                        u.Title,
                        u.createdOn,
                        IdentityDTO = u.identityUser == null ? null : new IdentityDTO
                        {
                            Email = u.identityUser.Email,
                            Username = u.identityUser.UserName,
                            Phone = u.identityUser.PhoneNumber,
                            TwoFactorEnabled = u.identityUser.TwoFactorEnabled,
                            AccessFailedCount = u.identityUser.AccessFailedCount,
                            LockoutEnabled = u.identityUser.LockoutEnabled
                        }
                    }
                ).FirstOrDefaultAsync();

            if (userModel == null)
            {
                return NotFound();
            }

            return Ok(userModel);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserModel(Guid id, UserModel userModel)
        {
            if (id != userModel.ID)
            {
                return BadRequest();
            }

            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUserModel(UserDTO userDTO)
        {
            var userModel = new UserModel
            {
                ID = Guid.NewGuid(),
                FirstName = userDTO.FirstName,
                Surname = userDTO.Surname,
                Gender = userDTO.Gender,
                Title = userDTO.Title,
                DOB = userDTO.DOB,
                UserID = userDTO.UserID.ToString(),
                createdOn = DateTime.UtcNow,
                UpdateOn = DateTime.UtcNow,
            };

            _context.UserProfiles.Add(userModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserModel", new { id = userModel.ID }, userModel);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserModel(Guid id)
        {
            var userModel = await _context.UserProfiles.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            _context.UserProfiles.Remove(userModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserModelExists(Guid id)
        {
            return _context.UserProfiles.Any(e => e.ID == id);
        }
    }
}
