using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;

namespace ColomBEE_CSharp_Relacional.API.Services{
    public class HealthCheckService(IHealthCheckRepository healthCheckRepository)
    {
        private readonly IHealthCheckRepository _healthCheckRepository = healthCheckRepository;

        public async Task<SaludSistema> GetHealthAsync()
        {
            var unEstadoSalud = await _healthCheckRepository
                .GetHealthAsync();
            
            return unEstadoSalud;
        }
    }
}