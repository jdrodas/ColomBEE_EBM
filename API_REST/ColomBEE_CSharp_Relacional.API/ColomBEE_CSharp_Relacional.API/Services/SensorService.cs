using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Exceptions;

namespace ColomBEE_CSharp_Relacional.API.Services
{

    public class SensorService(ISensorRepository sensorRepository)
    {
        private readonly ISensorRepository _sensorRepository = sensorRepository;

        public async Task<List<Sensor>> GetAllAsync()
        {
            return await _sensorRepository
                .GetAllAsync();
        }
        
        public async Task<Sensor> GetByIdAsync(Guid sensorId)
        {
            Sensor unSensor = await _sensorRepository
                .GetByIdAsync(sensorId);

            if (unSensor.Id == Guid.Empty)
                throw new EmptyCollectionException($"Sensor no encontrado con el Id {sensorId}");

            return unSensor;
        }
    }
}