using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelApi.Models;

namespace TravelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly TravelApiContext _db;

        public ReviewsController(TravelApiContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<List<Review>> Get(string title, string text, int rating)
        {
            IQueryable<Review> query = _db.Reviews.Include(m => m.Destination).AsQueryable();

            if (title != null)
            {
                query = query.Where(entry => entry.Title == title);
            }

            if (text != null)
            {
                query = query.Where(entry => entry.Text == text);
            }

            if (rating > 0)
            {
                query = query.Where(entry => entry.Rating == rating);
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            Review review = await _db.Reviews.Include(m => m.Destination).FirstOrDefaultAsync(m => m.ReviewId == id);
            //.AsNoTracking()
            //FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        [HttpPost]
        public async Task<ActionResult<Review>> Post(Review review)
        {
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReview), new
            {
                id = review.ReviewId
            }, review);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Review review)
        {
            if (id != review.ReviewId)
            {
                return BadRequest();
            }

            _db.Reviews.Update(review);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        private bool ReviewExists(int id)
        {
            return _db.Reviews.Any(e => e.ReviewId == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            Review review = await _db.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            _db.Reviews.Remove(review);
            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}
