using Asp.Versioning;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColomBEE_CSharp_Relacional.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/sensores")]
[Produces("application/json")]
public class SensoresController(SensorService sensorService) : Controller
{
    private readonly SensorService _sensorService = sensorService;

    [HttpGet]
    [ProducesResponseType(typeof(List<Sensor>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var losSensores = await _sensorService
                .GetAllAsync();

            return Ok(losSensores);
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

    [HttpGet("{sensorId:Guid}")]
    [ProducesResponseType(typeof(Sensor), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(Guid sensorId)
    {
        try
        {
            var unSensor = await _sensorService
                .GetByIdAsync(sensorId);

            return Ok(unSensor);
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
    
    [HttpGet("{sensorId:Guid}/lecturas")]
    [ProducesResponseType(typeof(List<Lectura>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAssociatedReadingsAsync(Guid sensorId)
    {
        try
        {
            var lasLecturasAsociadas = await _sensorService
                .GetAssociatedReadingsAsync(sensorId);

            return Ok(lasLecturasAsociadas);
        }
        catch (EmptyCollectionException error)
        {
            var unProblema = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Petición sin resultados",
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
    [ProducesResponseType(typeof(Sensor), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(Sensor unSensor)
    {
        try
        {
            var sensorCreado = await _sensorService
                .CreateAsync(unSensor);

            return StatusCode(StatusCodes.Status201Created, sensorCreado);
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
    
        [HttpPut]
    [ProducesResponseType(typeof(Sensor), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(Sensor unSensor)
    {
        try
        {
            var sensorActualizado = await _sensorService
                .UpdateAsync(unSensor);

            return StatusCode(StatusCodes.Status200OK, sensorActualizado);
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

    [HttpDelete("{sensorId:Guid}")]
    [ProducesResponseType(typeof(RespuestaApi), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(Guid sensorId)
    {
        try
        {
            var sensorEliminado = await _sensorService
                .RemoveAsync(sensorId);

            var unaRespuesta = new RespuestaApi
            {
                StatusCode = 200,
                Mensaje = sensorEliminado
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