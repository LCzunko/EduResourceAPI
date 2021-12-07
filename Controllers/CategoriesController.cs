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
    [Route("categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(ILogger<CategoriesController> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<CategoryReadDTO>>> GetCategories()
        {
            var categories = await _unitOfWork.Categories.Get(null!);
            if (categories is null || categories.Count == 0)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - No Categories Exist");
                return NotFound();
            }

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success - {categories.Count}");
            return Ok(_mapper.Map<ICollection<CategoryReadDTO>>(categories));
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryReadDTO>> GetCategories(int categoryId)
        {
            var categories = await _unitOfWork.Categories.Get(x => x.Id == categoryId);
            var category = categories.SingleOrDefault();
            if (category is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Category Not Found");
                return NotFound();
            }

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return Ok(_mapper.Map<ICollection<CategoryReadDTO>>(categories));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CategoryReadDTO>> PostCategories(CategoryCreateDTO category)
        {
            var categoryEntity = _mapper.Map<Category>(category);
            _unitOfWork.Categories.Insert(categoryEntity);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success - Id: {categoryEntity.Id}");
            return CreatedAtAction("GetCategories", new { categoryId = categoryEntity.Id }, _mapper.Map<CategoryReadDTO>(categoryEntity));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{categoryId}")]
        public async Task<ActionResult> PutCategories(int categoryId, CategoryCreateDTO category)
        {
            var categories = await _unitOfWork.Categories.Get(x => x.Id == categoryId);
            var categoryEntity = categories.SingleOrDefault();
            if (categoryEntity is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Category Not Found");
                return NotFound();
            }

            var updatedCategoryEntity = _mapper.Map(category, categoryEntity);
            _unitOfWork.Categories.Update(updatedCategoryEntity);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteCategories(int categoryId)
        {
            var categories = await _unitOfWork.Categories.Get(x => x.Id == categoryId);
            var category = categories.SingleOrDefault();
            if (category is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Category Not Found");
                return NotFound();
            }

            await _unitOfWork.Categories.Delete(categoryId);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return NoContent();
        }
    }
}
