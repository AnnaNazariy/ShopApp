using Dapper_Example.DAL.Repositories.Interfaces;
using Dapper_Example.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dapper_Example.DAL.Controllers
{ 
    [ApiController]
    [Route("/api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ILogger<ReviewController> _logger;
        private IUnitOfWork _unitofWork;

        public ReviewController(ILogger<ReviewController> logger, IUnitOfWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        // GET: api/Review/GetAllReviews
        [HttpGet("GetAllReviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetAllReviewsAsync()
        {
            try
            {
                var results = await _unitofWork.ReviewRepository.GetAllAsync();
                _unitofWork.Commit();
                _logger.LogInformation($"Returned all reviews from database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside GetAllReviewsAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Review/GetByProductId/id
        [HttpGet("GetByProductId/{id}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetByProductIdAsync(int id)
        {
            try
            {
                var results = await _unitofWork.ReviewRepository.ReviewsByProductAsync(id);
                _unitofWork.Commit();
                if (results == null)
                {
                    _logger.LogError($"No reviews found for product with id: {id}");
                    return NotFound();
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside GetByProductIdAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Review
        [HttpPost("PostReview")]
        public async Task<ActionResult> PostReviewAsync([FromBody] Review newReview)
        {
            try
            {
                if (newReview == null)
                {
                    _logger.LogError("Review object sent from client is null.");
                    return BadRequest("Review object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid review object sent from client.");
                    return BadRequest("Invalid review object");
                }

                var created_id = await _unitofWork.ReviewRepository.AddAsync(newReview);
                var CreatedReview = await _unitofWork.ReviewRepository.GetAsync(created_id);
                _unitofWork.Commit();
                return Ok(CreatedReview);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PostReviewAsync: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
