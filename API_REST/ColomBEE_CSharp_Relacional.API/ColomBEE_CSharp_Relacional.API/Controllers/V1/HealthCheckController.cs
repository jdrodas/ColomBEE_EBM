using Asp.Versioning;
using ColomBEE_CSharp_Relacional.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColomBEE_CSharp_Relacional.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/healthCheck")]
    public class HealthCheckController(HealthCheckService healthCheckService) : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService = healthCheckService;

        [HttpGet]
        public async Task<IActionResult> GetHealthAsync()
        {
            var unEstadoSalud = await _healthCheckService
                    .GetHealthAsync();
                
            return unEstadoSalud.EstaSaludable
                ? StatusCode(StatusCodes.Status200OK, unEstadoSalud)
                : StatusCode(StatusCodes.Status503ServiceUnavailable, unEstadoSalud);             
        }
    }
}