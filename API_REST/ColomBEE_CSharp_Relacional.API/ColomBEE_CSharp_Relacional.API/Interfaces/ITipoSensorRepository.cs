using ColomBEE_CSharp_Relacional.API.Models;

namespace ColomBEE_CSharp_Relacional.API.Interfaces
{
    public interface ITipoSensorRepository
    {
        public Task<List<TipoSensor>> GetAllAsync();
        public Task<TipoSensor> GetByIdAsync(Guid tipoSensorId);
    }
}