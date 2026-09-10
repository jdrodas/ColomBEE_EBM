using Asp.Versioning;
using ColomBEE_CSharp_Relacional.API.Services;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ColomBEE_CSharp_Relacional.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/sensores")]
    public class SensoresController(SensorService sensorService) : Controller
    {
        private readonly SensorService _sensorService = sensorService;
        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var losSensores = await _sensorService
                .GetAllAsync();

            return Ok(losSensores);
        }
        
        [HttpGet("{sensorId:Guid}")]
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
                return NotFound($"Error de validación: {error.Message}");
            }
        }
    }
}