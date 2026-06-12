using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.DTO;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly DBContext _context;

        public ReviewsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewModel>>> GetReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewModel>> GetReviewModel(Guid id)
        {
            var reviewModel = await _context.Reviews.FindAsync(id);

            if (reviewModel == null)
            {
                return NotFound();
            }

            return reviewModel;
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReviewModel(Guid id, ReviewModel reviewModel)
        {
            if (id != reviewModel.ID)
            {
                return BadRequest();
            }

            _context.Entry(reviewModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewModelExists(id))
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

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReviewModel>> PostReviewModel(ReviewDTO reviewModel)
        {
            var firstStatus = await _context.Statuses.OrderBy(s => s.SortOrder).FirstOrDefaultAsync();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            TrackingModel tracking = new TrackingModel()
            {
                StatusID = firstStatus.ID,
                ProjectID = reviewModel.ProjectID,
                userID = userId,
                assignedDate = DateTime.UtcNow,
                createdOn = DateTime.UtcNow,
                updatedOn = DateTime.UtcNow,
            };
            _context.Trackings.Add(tracking);
            await _context.SaveChangesAsync();
            ReviewModel model = new ReviewModel() 
            { 
                ID = reviewModel.ID,
                TrackingID = tracking.Id,
                Remarks = reviewModel.Remarks,
                Date = reviewModel.Date,
                createdOn = reviewModel.createdOn,
                updatedOn = reviewModel.updatedOn,
            };
            _context.Reviews.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReviewModel", new { id = reviewModel.ID }, reviewModel);
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviewModel(Guid id)
        {
            var reviewModel = await _context.Reviews.FindAsync(id);
            if (reviewModel == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(reviewModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewModelExists(Guid id)
        {
            return _context.Reviews.Any(e => e.ID == id);
        }
    }
}
