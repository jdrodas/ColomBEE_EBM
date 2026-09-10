using ColomBEE_CSharp_Relacional.API.Models;

namespace ColomBEE_CSharp_Relacional.API.Interfaces
{
    public interface ISensorRepository
    {
        public Task<List<Sensor>> GetAllAsync();
        public Task<Sensor> GetByIdAsync(Guid sensorId);
    }
}