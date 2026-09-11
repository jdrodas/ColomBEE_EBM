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
        
        public async Task<TipoSensor> CreateAsync(TipoSensor unTipoSensor)
        {
            unTipoSensor.Nombre = unTipoSensor.Nombre!.Trim();
            unTipoSensor.UnidadMedida = unTipoSensor.UnidadMedida!.Trim();
            
            string resultadoValidacion = EvaluateSensorTypeDetailsAsync(unTipoSensor);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var tipoSensorExistente = await _tipoSensorRepository
                .GetByDetailsAsync(unTipoSensor);

            if(tipoSensorExistente.Nombre!.ToUpper().Equals(unTipoSensor.Nombre.ToUpper()) &&
               tipoSensorExistente.UnidadMedida!.ToUpper().Equals(unTipoSensor.UnidadMedida.ToUpper()))
                return tipoSensorExistente;

            try
            {
                bool resultadoAccion = await _tipoSensorRepository
                    .CreateAsync(unTipoSensor);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                tipoSensorExistente = await _tipoSensorRepository
                    .GetByDetailsAsync(unTipoSensor);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return tipoSensorExistente;
        }

        public async Task<string> RemoveAsync(Guid tipoSensorId)
        {
            var respuesta = "resultado";
            
            var tipoSensorExistente = await _tipoSensorRepository
                .GetByIdAsync(tipoSensorId);

            if (tipoSensorExistente.Id == Guid.Empty)
                throw new EmptyCollectionException($"No hay un tipo de sensor con id {tipoSensorId}");

            try
            {
                var resultado = await _tipoSensorRepository
                    .RemoveAsync(tipoSensorId);
                
                if (resultado)
                    respuesta = $"Eliminado el tipo de sensor {tipoSensorExistente.Nombre} con unidad de medida {tipoSensorExistente.UnidadMedida}";
            }
            catch (DbOperationException)
            {
                throw;
            }

            return respuesta;
        }

        private static string EvaluateSensorTypeDetailsAsync(TipoSensor unTipoSensor)
        {
            if (string.IsNullOrEmpty(unTipoSensor.Nombre))
                return "No se puede insertar un tipo de sensor con nombre nulo";

            if (string.IsNullOrEmpty(unTipoSensor.UnidadMedida))
                return "No se puede insertar un tipo de sensor con unidad de medida nula";

            
            return string.Empty;
        }
    }
}