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
    [Produces("application/json")]
    public class ColmenasController(ColmenaService colmenaService) : Controller
    {
        private readonly ColmenaService _colmenaService = colmenaService;
        
        [HttpGet]
        [ProducesResponseType(typeof(List<Colmena>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var lasColmenas = await _colmenaService
                    .GetAllAsync();

                return Ok(lasColmenas);
            }
            catch (DbOperationException error)
            {
                var unProblema = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error en bases de datos",
                    Detail = error.Message
                };
                
                return StatusCode(StatusCodes.Status500InternalServerError, unProblema);
            }

        }
        
        [HttpGet("{colmenaId:Guid}")]
        [ProducesResponseType(typeof(Colmena), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

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
                var unProblema = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Registro no encontrado con ese Id",
                    Detail = error.Message
                };
                
                return StatusCode(StatusCodes.Status404NotFound, unProblema);
            }
            catch (DbOperationException error)
            {
                var unProblema = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error en bases de datos",
                    Detail = error.Message
                };
                
                return StatusCode(StatusCodes.Status500InternalServerError, unProblema);
            }
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Colmena), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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
                var unProblema = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error en aplicación al procesar solicitud",
                    Detail = error.Message
                };
                
                return StatusCode(StatusCodes.Status400BadRequest, unProblema);
            }
            catch (DbOperationException error)
            {
                var unProblema = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error en bases de datos",
                    Detail = error.Message
                };
                
                return StatusCode(StatusCodes.Status500InternalServerError, unProblema);
            }
            catch (ConflictException error)
            {
                var unProblema = new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflicto al procesar la solicitud",
                    Detail = error.Message
                };
                
                return StatusCode(StatusCodes.Status409Conflict, unProblema);
            }
        }
    }
}