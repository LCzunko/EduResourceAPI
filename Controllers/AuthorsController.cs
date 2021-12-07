using AutoMapper;
using EduResourceAPI.Controllers.Errors;
using EduResourceAPI.Data;
using EduResourceAPI.Models.DTOs;
using EduResourceAPI.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EduResourceAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [Route("authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ILogger<AuthorsController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthorsController(ILogger<AuthorsController> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<AuthorReadDTO>>> GetAuthors()
        {
            var authors = await _unitOfWork.Authors.Get(null!, "Materials");
            if (authors is null || authors.Count == 0)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - No Authors Exist");
                return NotFound();
            }

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success - {authors.Count}");
            return Ok(_mapper.Map<ICollection<AuthorReadDTO>>(authors));
        }

        [HttpGet("{authorId}")]
        public async Task<ActionResult<AuthorReadDTO>> GetAuthors(int authorId)
        {
            var authors = await _unitOfWork.Authors.Get(x => x.Id == authorId, "Materials");
            var author = authors.SingleOrDefault();
            if (author is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Author Not Found");
                return NotFound();
            }

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return Ok(_mapper.Map<ICollection<AuthorReadDTO>>(authors));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<AuthorReadDTO>> PostAuthors(AuthorCreateDTO author)
        {
            var authorEntity = _mapper.Map<Author>(author);
            _unitOfWork.Authors.Insert(authorEntity);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success - Id: {authorEntity.Id}");
            return CreatedAtAction("GetAuthors", new { authorId = authorEntity.Id }, _mapper.Map<AuthorReadDTO>(authorEntity));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{authorId}")]
        public async Task<ActionResult> PutAuthors(int authorId, AuthorCreateDTO author)
        {
            var authors = await _unitOfWork.Authors.Get(x => x.Id == authorId, "Materials");
            var authorEntity = authors.SingleOrDefault();
            if (authorEntity is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Author Not Found");
                return NotFound();
            }

            var updatedAuthorEntity = _mapper.Map(author, authorEntity);
            _unitOfWork.Authors.Update(updatedAuthorEntity);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{authorId}")]
        public async Task<ActionResult> DeleteAuthors(int authorId)
        {
            var authors = await _unitOfWork.Authors.Get(x => x.Id == authorId);
            var author = authors.SingleOrDefault();
            if (author is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Author Not Found");
                return NotFound();
            }

            await _unitOfWork.Authors.Delete(authorId);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return NoContent();
        }
    }
}
