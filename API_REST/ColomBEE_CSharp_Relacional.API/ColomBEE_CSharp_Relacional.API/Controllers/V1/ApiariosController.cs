using Asp.Versioning;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColomBEE_CSharp_Relacional.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/apiarios")]
[Produces("application/json")]
public class ApiariosController(ApiarioService apiarioService) : Controller
{
    private readonly ApiarioService _apiarioService = apiarioService;

    [HttpGet]
    [ProducesResponseType(typeof(List<Apiario>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var losAPiarios = await _apiarioService
                .GetAllAsync();

            return Ok(losAPiarios);
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

    [HttpGet("{apiarioId:Guid}")]
    [ProducesResponseType(typeof(Apiario), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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
    [HttpGet("{apiarioId:Guid}/colmenas")]
    [ProducesResponseType(typeof(List<Colmena>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAssociatedBeehivesAsync(Guid apiarioId)
    {
        try
        {
            var lasColmenasAsociadas = await _apiarioService
                .GetAssociatedBeehivesAsync(apiarioId);

            return Ok(lasColmenasAsociadas);
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
    [ProducesResponseType(typeof(Apiario), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(Apiario unApiario)
    {
        try
        {
            var apiarioCreado = await _apiarioService
                .CreateAsync(unApiario);

            return StatusCode(StatusCodes.Status201Created, apiarioCreado);
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
    [ProducesResponseType(typeof(Apiario), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(Apiario unApiario)
    {
        try
        {
            var apiarioActualizado = await _apiarioService
                .UpdateAsync(unApiario);

            return StatusCode(StatusCodes.Status200OK, apiarioActualizado);
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

    [HttpDelete("{apiarioId:Guid}")]
    [ProducesResponseType(typeof(RespuestaApi), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(Guid apiarioId)
    {
        try
        {
            var apiarioEliminado = await _apiarioService
                .RemoveAsync(apiarioId);

            var unaRespuesta = new RespuestaApi
            {
                StatusCode = 200,
                Mensaje = apiarioEliminado
            };

            return Ok(unaRespuesta);
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
    }
}