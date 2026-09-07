using ColomBEE_CSharp_Relacional.API.Models;

namespace ColomBEE_CSharp_Relacional.API.Interfaces
{
    public interface IHealthCheckRepository
    {
        public Task<SaludSistema> GetHealthAsync();
    }
}