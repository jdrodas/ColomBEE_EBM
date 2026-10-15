using Asp.Versioning;
using ColomBEE_CSharp_NoSQL.API.Services;
using ColomBEE_CSharp_NoSQL.API.Models;
using ColomBEE_CSharp_NoSQL.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ColomBEE_CSharp_NoSQL.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/estadisticas")]
[Produces("application/json")]
public class EstadisticasController(EstadisticaService estadisticaService) : Controller
{
    private readonly EstadisticaService _estadisticaService = estadisticaService;

    [HttpGet]
    [ProducesResponseType(typeof(Estadistica), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var lasEstadisticas = await _estadisticaService
                .GetAllAsync();

            return Ok(lasEstadisticas); 
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
}