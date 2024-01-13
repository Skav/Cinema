using Cinema.DTO;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Security.Claims;
using System.Text.Json;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace Cinema.Controllers
{
    [Route("api/coupons")]
    [ApiController]
    public class CouponController : BaseController
    {

        public CouponController(AppDbContext context) : base(context){ }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getUserCoupons()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userCoupons = await _context.Coupons.Where(x => x.userId == userId).Where(x => x.active == true).ToListAsync();

            if (userCoupons == null || userCoupons.Count() == 0)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(userCoupons);
        }

        [HttpGet]
        [Route("{userId}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> getUserCouponsById(string userId)
        {
            var userCoupons = await _context.Coupons.Where(x => x.userId == userId).ToListAsync();

            if (userCoupons == null || userCoupons.Count() == 0)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(userCoupons);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> addCoupon([FromBody] CouponsDTO request)
        {
            if (request.userId == null)
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "You need to specify userId!"
                }));

            if (request == null || request.discount == null || request.discountType == null || request.userId == null)
                return BadRequest();


            var coupon = mapper.Map<CouponsModel>(request);
            coupon.dateAdded = DateTime.UtcNow;
            coupon.expDate = DateTime.UtcNow.AddDays(30);
            coupon.active = true;

            await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();

            return Created("", coupon);
        }

        [HttpPut]
        [Route("{couponId:int}/edit")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> updateCoupon(int couponId, [FromBody] CouponsDTO request)
        {
            if (request == null || couponId == null)
                return BadRequest();

            var dbObject = await _context.Coupons.Where(x => x.id == couponId).FirstOrDefaultAsync();

            if (dbObject == null)
                return NotFound();

            _context.Entry(dbObject).CurrentValues.SetValues(request);
            await _context.SaveChangesAsync();

            return Accepted(dbObject);
        }

        [HttpDelete]
        [Route("{couponId:int}/delete")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> deleteCoupon(int couponId, bool forceDelete = false)
        {
            if(couponId == null)
                return BadRequest();

            var coupon = await _context.Coupons.Where(x => x.id == couponId).FirstOrDefaultAsync();

            if (coupon == null)
                return NotFound(nameof(coupon));

            if (forceDelete)
                await _context.Coupons.Where(x => x.id == couponId).ExecuteDeleteAsync();
            else
            {
                _context.Entry(coupon).CurrentValues.SetValues(new {active = false});
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
        

    }
}
