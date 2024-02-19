using LMSystem.Repository.Data;
using LMSystem.Repository.Models;
using LMSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingCourseController : Controller
    {
        private readonly IRatingCourseService _ratingCourseService;

        public RatingCourseController(IRatingCourseService ratingCourseService)
        {
            _ratingCourseService = ratingCourseService;
        }

        [HttpPost("RatingCourse")]
        public async Task<ActionResult> AddRating([FromQuery] AddRatingModel addRatingModel, [FromQuery] int registrationId)
        {
            // Assuming the user's ID is determined from the context (e.g., authenticated user)
            // and you have a way to associate the rating with a registration/course

            var ratingCourse = new RatingCourse
            {
                RegistrationId = registrationId, // You need to ensure this ID is correctly handled
                RatingStar = addRatingModel.RatingStar,
                CommentContent = addRatingModel.CommentContent,
                RatingDate = DateTime.UtcNow, // Set the rating date to now
                IsRatingStatus = true // Assuming you want to set a default value
            };

            var addedRating = await _ratingCourseService.AddRatingAsync(ratingCourse);
            return CreatedAtAction(nameof(AddRating), new { id = addedRating.RatingId }, addedRating);
        }
    }
}
