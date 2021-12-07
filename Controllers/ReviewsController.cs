using AutoMapper;
using EduResourceAPI.Data;
using EduResourceAPI.Models.DTOs;
using EduResourceAPI.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduResourceAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [Route("materials/{materialId}/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ILogger<ReviewsController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewsController(ILogger<ReviewsController> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<ReviewReadDTO>>> GetReviews(int materialId)
        {
            var reviews = await _unitOfWork.Reviews.Get(x => x.MaterialId == materialId, "Material");
            if (reviews is null || reviews.Count == 0)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - No Reviews Exist");
                return NotFound();
            }

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success - {reviews.Count}");
            return Ok(_mapper.Map<ICollection<ReviewReadDTO>>(reviews));
        }

        [HttpGet("{reviewId}")]
        public async Task<ActionResult<ReviewReadDTO>> GetReviews(int materialId, int reviewId)
        {
            var reviews = await _unitOfWork.Reviews.Get(x => x.MaterialId == materialId && x.Id == reviewId, "Material");
            var review = reviews.SingleOrDefault();
            if (review is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Review Not Found");
                return NotFound();
            }

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return Ok(_mapper.Map<ICollection<ReviewReadDTO>>(reviews));
        }

        [HttpPost]
        public async Task<ActionResult<ReviewReadDTO>> PostReviews(int materialId, ReviewCreateDTO review)
        {
            var reviewEntity = _mapper.Map<Review>(review);
            reviewEntity.MaterialId = materialId;
            _unitOfWork.Reviews.Insert(reviewEntity);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success - Id: {reviewEntity.Id}");
            return CreatedAtAction("GetReviews", new { materialId = materialId, reviewId = reviewEntity.Id }, _mapper.Map<ReviewReadDTO>(reviewEntity));
        }

        [HttpPut("{reviewId}")]
        public async Task<ActionResult> PutReviews(int materialId, int reviewId, ReviewCreateDTO review)
        {
            var reviews = await _unitOfWork.Reviews.Get(x => x.MaterialId == materialId && x.Id == reviewId, "Material");
            var reviewEntity = reviews.SingleOrDefault();
            if (reviewEntity is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Review Not Found");
                return NotFound();
            }

            var updatedReviewEntity = _mapper.Map(review, reviewEntity);
            updatedReviewEntity.MaterialId = materialId;
            _unitOfWork.Reviews.Update(updatedReviewEntity);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{reviewId}")]
        public async Task<ActionResult> DeleteReviews(int materialId, int reviewId)
        {
            var reviews = await _unitOfWork.Reviews.Get(x => x.MaterialId == materialId && x.Id == reviewId);
            var review = reviews.SingleOrDefault();
            if (review is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Review Not Found");
                return NotFound();
            }

            await _unitOfWork.Reviews.Delete(reviewId);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return NoContent();
        }
    }
}
