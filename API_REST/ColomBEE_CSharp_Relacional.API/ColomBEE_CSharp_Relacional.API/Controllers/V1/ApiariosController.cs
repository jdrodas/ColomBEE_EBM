using Asp.Versioning;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Services;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ColomBEE_CSharp_Relacional.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/apiarios")]
    public class ApiariosController(ApiarioService apiarioService) : Controller
    {
        private readonly ApiarioService _apiarioService = apiarioService;
        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losAPiarios = await _apiarioService
                .GetAllAsync();

            return Ok(losAPiarios);
        }
        
        [HttpGet("{apiarioId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid apiarioId)
        {
            try
            {
                var unApiario = await _apiarioService
                    .GetByIdAsync(apiarioId);

                return Ok(unApiario);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Apiario unApiario)
        {
            try
            {
                var apiarioCreado = await _apiarioService
                    .CreateAsync(unApiario);

                return Ok(apiarioCreado);
            }
            catch (AppValidationException error)
            {
                return BadRequest($"Error de validación: {error.Message}");
            }
            catch (DbOperationException error)
            {
                return BadRequest($"Error de operacion en DB: {error.Message}");
            }
        }
    }
}