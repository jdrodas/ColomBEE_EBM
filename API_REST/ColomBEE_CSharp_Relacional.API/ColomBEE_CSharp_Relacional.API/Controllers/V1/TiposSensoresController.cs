using Asp.Versioning;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColomBEE_CSharp_Relacional.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/tiposSensores")]
[Produces("application/json")]
public class TiposSensoresController(TipoSensorService tipoSensorService) : Controller
{
    private readonly TipoSensorService _tipoSensorService = tipoSensorService;

    [HttpGet]
    [ProducesResponseType(typeof(List<TipoSensor>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var losTiposSensores = await _tipoSensorService
                .GetAllAsync();

            return Ok(losTiposSensores);
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

    [HttpGet("{tipoSensorId:Guid}")]
    [ProducesResponseType(typeof(TipoSensor), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(Guid tipoSensorId)
    {
        try
        {
            var unTipoSensor = await _tipoSensorService
                .GetByIdAsync(tipoSensorId);

            return Ok(unTipoSensor);
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
    [ProducesResponseType(typeof(TipoSensor), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(TipoSensor unTipoSensor)
    {
        try
        {
            var tipoSensorCreado = await _tipoSensorService
                .CreateAsync(unTipoSensor);

            return StatusCode(StatusCodes.Status201Created, tipoSensorCreado);
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

    [HttpDelete("{tipoSensorId:Guid}")]
    [ProducesResponseType(typeof(TipoSensor), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(Guid tipoSensorId)
    {
        try
        {
            var tipoSensorEliminado = await _tipoSensorService
                .RemoveAsync(tipoSensorId);

            var unaRespuesta = new RespuestaApi
            {
                StatusCode = 200,
                Mensaje = tipoSensorEliminado
            };

            return Ok(unaRespuesta);
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
}