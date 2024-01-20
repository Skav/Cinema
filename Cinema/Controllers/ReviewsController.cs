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

        [HttpGet]
        [Route("user")]
        [Authorize]
        public async Task<IActionResult> getUserReviews()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
                return Unauthorized();

            var query = from reviews in _context.Reviews
                        join users in _context.Users on reviews.userId equals users.Id
                        join movies in _context.Movies on reviews.movieId equals movies.id
                        where reviews.userId == userId
                        select new
                        {
                            reviews.id,
                            reviews.rating,
                            reviews.content,
                            reviews.movieId,
                            movies.title
                        };

            var result = await query.ToListAsync();


            if (result.Count == 0)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(result);
        }

        [HttpGet]
        [Route("movie/{movieId:int}/user")]
        [Authorize]
        public async Task<IActionResult> getReviewByMovieIdDetailsForUser(int movieId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
                return Unauthorized();

            var query = from reviews in _context.Reviews
                        join users in _context.Users on reviews.userId equals users.Id
                        join movies in _context.Movies on reviews.movieId equals movies.id
                        where reviews.movieId == movieId
                        where userId == userId
                        select new
                        {
                            reviews.id,
                            reviews.rating,
                            reviews.content,
                            reviews.movieId,
                            users.UserName,
                            movies.title
                        };

            var result = await query.FirstOrDefaultAsync();


            if (result == null)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(result);
        }

        [HttpGet]
        [Route("movie/{movieId:int}")]
        public async Task<IActionResult> getReviewByMovieIdDetails(int movieId)
        {
            var query = from reviews in _context.Reviews
                        join users in _context.Users on reviews.userId equals users.Id
                        join movies in _context.Movies on reviews.movieId equals movies.id
                        where reviews.movieId == movieId
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
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "You need to add content!"
                }));

            if (request.rating == null || request.rating < 0 || request.rating > 10)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Wron rating value"
                }));

            if (!await _context.Users.Where(x => x.Id == request.userId).AnyAsync())
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "User with given ID doesn't exists!"
                }));

            if (!await _context.Movies.Where(x => x.id == request.movieId).AnyAsync())
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Movie with given ID doesn't exists!"
                }));

            if (await _context.Reviews.Where(x => x.userId == request.userId).Where(x => x.movieId == request.movieId).AnyAsync())
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "User already added review to this movie"
                }));

            var review = mapper.Map<ReviewsModel>(request);
            review.dateAdded = DateTime.UtcNow;

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            return Created("", review);
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
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "User with given ID doesn't exists!"
                }));

            var dbObject = await _context.Reviews.Where(x => x.id == reviewId).FirstOrDefaultAsync();

            if (dbObject == null)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Review doesn't exists!"
                }));

            request.movieId = dbObject.movieId;

            _context.Entry(dbObject).CurrentValues.SetValues(request);
            await _context.SaveChangesAsync();

            return Ok(dbObject);
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
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "User with given ID doesn't exists!"
                }));

            var review = await _context.Reviews.Where(x => x.id == reviewId).FirstOrDefaultAsync();

            if (reviewId == null)
                return Conflict(JsonSerializer.Serialize(new {
                    error = "Review doesn't exists!"
                }));

            if (review.userId != userId)
                return Unauthorized(JsonSerializer.Serialize(new
                {
                    error = "You are not authorized to delete this review"
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
                return Conflict(JsonSerializer.Serialize(new {
                    error = "Review doesn't exists!"
                }));

            await _context.Reviews.Where(x => x.id == reviewId).ExecuteDeleteAsync();

            return Ok();
        }
    }
}
