﻿using AutoMapper;
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
    [Route("materials")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly ILogger<MaterialsController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MaterialsController(ILogger<MaterialsController> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<MaterialReadDTO>>> GetMaterials()
        {
            var materials = await _unitOfWork.Materials.Get(null!, "Author,Category");
            if (materials is null || materials.Count == 0)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - No Materials Exist");
                return NotFound();
            }

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success - {materials.Count}");
            return Ok(_mapper.Map<ICollection<MaterialReadDTO>>(materials));
        }

        [HttpGet("{materialId}")]
        public async Task<ActionResult<MaterialReadDTO>> GetMaterials(int materialId)
        {
            var materials = await _unitOfWork.Materials.Get(x => x.Id == materialId, "Author,Category");
            var material = materials.SingleOrDefault();
            if (material is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Material Not Found");
                return NotFound();
            }

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return Ok(_mapper.Map<ICollection<MaterialReadDTO>>(materials));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MaterialReadDTO>> PostMaterials(MaterialCreateDTO material)
        {
            var authors = await _unitOfWork.Authors.Get(x => x.Id == material.AuthorId);
            var author = authors.SingleOrDefault();
            if (author is null)
            {
                Dictionary<string, string> errors = new() { { "AuthorId", "Author does not exist in database." } };
                var traceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - {string.Join(" | ", errors)}");
                return BadRequest(new BadRequestError(traceId, errors));
            }

            var categories = await _unitOfWork.Categories.Get(x => x.Id == material.CategoryId);
            var category = authors.SingleOrDefault();
            if (category is null)
            {
                Dictionary<string, string> errors = new() { { "CategoryId", "Category does not exist in database." } };
                var traceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - {string.Join(" | ", errors)}");
                return BadRequest(new BadRequestError(traceId, errors));
            }

            var materialEntity = _mapper.Map<Material>(material);
            _unitOfWork.Materials.Insert(materialEntity);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success - Id: {materialEntity.Id}");
            return CreatedAtAction("GetMaterials", new { materialId = materialEntity.Id }, _mapper.Map<MaterialReadDTO>(materialEntity));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPut("{materialId}")]
        public async Task<ActionResult> PutMaterials(int materialId, MaterialCreateDTO material)
        {
            var materials = await _unitOfWork.Materials.Get(x => x.Id == materialId, "Author,Category");
            var materialEntity = materials.SingleOrDefault();
            if (materialEntity is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Material Not Found");
                return NotFound();
            }

            var authors = await _unitOfWork.Authors.Get(x => x.Id == material.AuthorId);
            var author = authors.SingleOrDefault();
            if (author is null)
            {
                Dictionary<string, string> errors = new() { { "AuthorId", "Author does not exist in database." } };
                var traceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - {string.Join(" | ", errors)}");
                return BadRequest(new BadRequestError(traceId, errors));
            }

            var categories = await _unitOfWork.Categories.Get(x => x.Id == material.CategoryId);
            var category = authors.SingleOrDefault();
            if (category is null)
            {
                Dictionary<string, string> errors = new() { { "CategoryId", "Category does not exist in database." } };
                var traceId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - {string.Join(" | ", errors)}");
                return BadRequest(new BadRequestError(traceId, errors));
            }

            var updatedMaterialEntity = _mapper.Map(material, materialEntity);
            _unitOfWork.Materials.Update(updatedMaterialEntity);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{materialId}")]
        public async Task<ActionResult> DeleteMaterials(int materialId)
        {
            var materials = await _unitOfWork.Materials.Get(x => x.Id == materialId);
            var material = materials.SingleOrDefault();
            if (material is null)
            {
                _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Fail - Material Not Found");
                return NotFound();
            }

            await _unitOfWork.Materials.Delete(materialId);
            bool result = await _unitOfWork.Commit();
            if (result is false) return BadRequest();

            _logger.LogInformation($"{Request.Method} {Request.Path.Value} - Success");
            return NoContent();
        }
    }
}
