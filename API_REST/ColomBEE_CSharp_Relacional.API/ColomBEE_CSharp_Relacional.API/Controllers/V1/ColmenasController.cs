using Asp.Versioning;
using ColomBEE_CSharp_Relacional.API.Services;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ColomBEE_CSharp_Relacional.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/colmenas")]
    public class ColmenasController(ColmenaService colmenaService) : Controller
    {
        private readonly ColmenaService _colmenaService = colmenaService;
        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var lasColmenas = await _colmenaService
                .GetAllAsync();

            return Ok(lasColmenas);
        }
        
        [HttpGet("{colmenaId:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid colmenaId)
        {
            try
            {
                var unaColmena = await _colmenaService
                    .GetByIdAsync(colmenaId);

                return Ok(unaColmena);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound($"Error de validación: {error.Message}");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Colmena unaColmena)
        {
            try
            {
                var colmenaCreada = await _colmenaService
                    .CreateAsync(unaColmena);

                return Ok(colmenaCreada);
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