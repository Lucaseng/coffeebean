using CoffeebeanAPI.Dtos;
using CoffeebeanAPI.Models;
using CoffeebeanAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeebeanAPI.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class ReviewController : Controller
    {
        private readonly MongoReviewService _mongoDBReviewService;

        public ReviewController(MongoReviewService mongoReviewService)
        {
            _mongoDBReviewService = mongoReviewService;
        }

        [HttpGet]
        public async Task<List<Review>> Get()
        {
            List<Review> myReviews = await _mongoDBReviewService.GetReviewsAsync();
            return myReviews;

        }


        [HttpPost]
        public async Task<IActionResult> PostReview(ReviewInput review)
        {
            Review createdReview = await _mongoDBReviewService.PostReviewAsync(review);
            //return Ok(createdReview);
            if (createdReview != null)
            {
                return CreatedAtAction(nameof(Get), new { createdReview.Id }, createdReview);
            } else
            {
                return BadRequest(new {fail = "Either the UserID, CoffeeId or CoffeeShopId was incorrect."});
            }

        }

    }
}
