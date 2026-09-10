using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Exceptions;

namespace ColomBEE_CSharp_Relacional.API.Services
{

    public class TipoSensorService(ITipoSensorRepository tipoSensorRepository)
    {
        private readonly ITipoSensorRepository _tipoSensorRepository = tipoSensorRepository;

        public async Task<List<TipoSensor>> GetAllAsync()
        {
            return await _tipoSensorRepository
                .GetAllAsync();
        }
        
        public async Task<TipoSensor> GetByIdAsync(Guid tipoSensorId)
        {
            TipoSensor unTipoSensor = await _tipoSensorRepository
                .GetByIdAsync(tipoSensorId);

            if (unTipoSensor.Id == Guid.Empty)
                throw new EmptyCollectionException($"Tipo Sensor no encontrado con el Id {tipoSensorId}");

            return unTipoSensor;
        }
    }
}