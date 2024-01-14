using Cinema.DTO;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using System.Text.Json;

namespace Cinema.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : BaseController
    {
        public ReviewsController(AppDbContext context) : base(context) { }


        [HttpGet]
        public async Task<IActionResult> getReviews()
        {
            var query = from reviews in _context.Reviews
                        join users in _context.Users on reviews.userId equals users.Id
                        join movies in _context.Movies on reviews.movieId equals movies.id
                        select new
                        {
                            reviews.id,
                            reviews.rating,
                            reviews.content,
                            reviews.movieId,
                            users.UserName,
                            movies.title
                        };

            var result = await query.ToListAsync();


            if (result.Count == 0)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(result);
        }

        [HttpGet]
        [Route("{revieId:int}")]
        public async Task<IActionResult> getReviewDetails(int revieId)
        {
            var query = from reviews in _context.Reviews
                        join users in _context.Users on reviews.userId equals users.Id
                        join movies in _context.Movies on reviews.movieId equals movies.id
                        where reviews.id == revieId
                        select new
                        {
                            reviews.id,
                            reviews.rating,
                            reviews.content,
                            reviews.movieId,
                            users.UserName,
                            movies.title
                        };

            var result = await query.ToListAsync();


            if (result.Count == 0)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> addReviewByUser([FromBody] ReviewsDTO request)
        {
            request.userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (request.userId == null)
                return Unauthorized();

            if (request.content == null)
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "You need to add content!"
                }));

            if (request.rating == null || request.rating < 0 || request.rating > 10)
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "Wron rating value"
                }));

            if (!await _context.Users.Where(x => x.Id == request.userId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "User with given ID doesn't exists!"
                }));

            if (!await _context.Movies.Where(x => x.id == request.movieId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "Movie with given ID doesn't exists!"
                }));

            var review = mapper.Map<ReviewsModel>(request);
            review.dateAdded = DateTime.UtcNow;

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }

        [HttpPut]
        [Route("{revieId:int}/edit")]
        [Authorize]
        public async Task<IActionResult> updateReviewByUser(int reviewId, [FromBody] ReviewsDTO request)
        {
            request.userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (request.userId == null)
                return Unauthorized();


            if (!await _context.Users.Where(x => x.Id == request.userId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "User with given ID doesn't exists!"
                }));

            var dbObject = await _context.Reviews.Where(x => x.id == reviewId).FirstOrDefaultAsync();

            if (dbObject == null)
                return NotFound();

            request.movieId = dbObject.movieId;

            _context.Entry(dbObject).CurrentValues.SetValues(request);
            await _context.SaveChangesAsync();

            return Accepted(dbObject);
        }

        [HttpDelete]
        [Route("{reviewId:int}/delete")]
        [Authorize]
        public async Task<IActionResult> deleteReviewByUser(int reviewId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
                return Unauthorized();


            if (!await _context.Users.Where(x => x.Id == userId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "User with given ID doesn't exists!"
                }));

            var review = await _context.Reviews.Where(x => x.id == reviewId).FirstOrDefaultAsync();

            if (reviewId == null)
                return NotFound(nameof(reviewId));

            if (review.userId != userId)
                return Unauthorized(JsonSerializer.Serialize(new
                {
                    Error = "You are not authorized to delete this review"
                }));


            await _context.Reviews.Where(x => x.id == reviewId).ExecuteDeleteAsync();


            return Ok();
        }

        [HttpDelete]
        [Route("{reviewId:int}/adminDelete")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> deleteReview(int reviewId)
        {
            var review = await _context.Reviews.Where(x => x.id == reviewId).FirstOrDefaultAsync();

            if (reviewId == null)
                return NotFound(nameof(reviewId));

            await _context.Reviews.Where(x => x.id == reviewId).ExecuteDeleteAsync();

            return Ok();
        }
    }
}
