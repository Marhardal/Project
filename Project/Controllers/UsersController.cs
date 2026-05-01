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
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;

        public UsersController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUserProfils()
        {
            return await _context.UserProfils.ToListAsync();
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
        public async Task<ActionResult<UserModel>> GetUserProfile(Guid userID)
        {
            var userModel = await _context.UserProfils.Where(u => u.UserID == userID.ToString()).FirstOrDefaultAsync();

            if (userModel == null)
            {
                return NotFound();
            }

            return userModel;
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

            _context.UserProfils.Add(userModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserModel", new { id = userModel.ID }, userModel);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserModel(Guid id)
        {
            var userModel = await _context.UserProfils.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            _context.UserProfils.Remove(userModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserModelExists(Guid id)
        {
            return _context.UserProfils.Any(e => e.ID == id);
        }
    }
}
