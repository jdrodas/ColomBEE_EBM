using Asp.Versioning;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Services;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ColomBEE_CSharp_Relacional.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/tiposSensores")]
    public class TiposSensoresController(TipoSensorService tipoSensorService) : Controller
    {
        private readonly TipoSensorService _tipoSensorService = tipoSensorService;
        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losTiposSensores = await _tipoSensorService
                .GetAllAsync();

            return Ok(losTiposSensores);
        }
        
        [HttpGet("{tipoSensorId:Guid}")]
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
                return NotFound($"Error de validación: {error.Message}");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync(TipoSensor unTipoSensor)
        {
            try
            {
                var tipoSensorCreado = await _tipoSensorService
                    .CreateAsync(unTipoSensor);

                return Ok(tipoSensorCreado);
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

        [HttpDelete("{tipoSensorId:Guid}")]
        public async Task<IActionResult> RemoveAsync(Guid tipoSensorId)
        {
            try
            {
                var tipoSensorEliminado = await _tipoSensorService
                    .RemoveAsync(tipoSensorId);

                return Ok(tipoSensorEliminado);
            }
            catch (EmptyCollectionException error)
            {
                return NotFound(error.Message);
            }
        }
    }
}