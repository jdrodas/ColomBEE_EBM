using ColomBEE_CSharp_Relacional.API.Models;

namespace ColomBEE_CSharp_Relacional.API.Interfaces
{
    public interface ITipoSensorRepository
    {
        public Task<List<TipoSensor>> GetAllAsync();
        public Task<TipoSensor> GetByIdAsync(Guid tipoSensorId);
        
        public Task<TipoSensor> GetByDetailsAsync(TipoSensor unTipoSensor);
        public Task<bool> CreateAsync(TipoSensor unTipoSensor);
        public Task<bool> RemoveAsync(Guid tipoSensorId);
    }
}