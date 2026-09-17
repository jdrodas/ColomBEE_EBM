using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using System.Globalization;

namespace ColomBEE_CSharp_Relacional.API.Services
{

    public class SensorService(ISensorRepository sensorRepository,
                                IColmenaRepository colmenaRepository,
                                ITipoSensorRepository tipoSensorRepository)
    {
        private readonly ISensorRepository _sensorRepository = sensorRepository;
        private readonly IColmenaRepository _colmenaRepository = colmenaRepository;
        private readonly ITipoSensorRepository _tipoSensorRepository = tipoSensorRepository;

        public async Task<List<Sensor>> GetAllAsync()
        {
            try
            {
                return await _sensorRepository 
                    .GetAllAsync(); 
            }
            catch (DbOperationException)
            {
                throw;
            }
        }

        public async Task<Sensor> GetByIdAsync(Guid sensorId)
        {
            try
            {
                Sensor unSensor = await _sensorRepository
                    .GetByIdAsync(sensorId);

                if (unSensor.Id == Guid.Empty)
                    throw new EmptyCollectionException($"Sensor no encontrado con el Id {sensorId}");

                return unSensor;
            }
            catch (DbOperationException)
            {
                throw;
            }
        }
        
        public async Task<Sensor> CreateAsync(Sensor unSensor)
        {
            unSensor.FechaInstalacion = unSensor.FechaInstalacion!.Trim();

            string resultadoValidacion = EvaluateSensorDetailsAsync(unSensor);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var colmenaExistente = await _colmenaRepository
                .GetByIdAsync(unSensor.ColmenaId);
            
            if (colmenaExistente.Id == Guid.Empty)
                throw new AppValidationException($"No existe colmena con Id {unSensor.ColmenaId}");

            unSensor.ColmenaCodigo = colmenaExistente.Codigo;
            
            var tipoSensorExistente = await _tipoSensorRepository
                .GetByIdAsync(unSensor.TipoId);
            
            if (tipoSensorExistente.Id == Guid.Empty)
                throw new AppValidationException($"No existe un tipo de sensor con Id {unSensor.TipoId}");
            
            var sensorExistente = await _sensorRepository
                .GetByDetailsAsync(unSensor);

            if (sensorExistente.Id != Guid.Empty)
                throw new ConflictException($"No se puede crear el sensor del tipo {tipoSensorExistente.Nombre} " +
                                            $"porque ya existe con id {sensorExistente.Id}");
            
            try
            {
                bool resultadoAccion = await _sensorRepository
                    .CreateAsync(unSensor);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                sensorExistente = await _sensorRepository
                    .GetByDetailsAsync(unSensor);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return sensorExistente;
        }
        
        public async Task<string> RemoveAsync(Guid sensorId)
        {
            var respuesta = "resultado";
            
            var sensorExistente = await _sensorRepository
                .GetByIdAsync(sensorId);

            if (sensorExistente.Id == Guid.Empty)
                throw new EmptyCollectionException($"No hay un sensor con id {sensorId}");

            var totalLecturasAsociadas = await _sensorRepository
                .GetTotalAssociatedReadingsAsync(sensorId);
            
            if(totalLecturasAsociadas >0)
                throw new AppValidationException($"El sensor con id {sensorId} tiene {totalLecturasAsociadas} lecturas asociadas. No se puede eliminar");
            
            try
            {
                var resultado = await _sensorRepository
                    .RemoveAsync(sensorId);
                
                if (resultado)
                    respuesta = $"Eliminado sensor del tipo {sensorExistente.TipoNombre} con Id {sensorExistente.Id} ha sido eliminado";
            }
            catch (DbOperationException)
            {
                throw;
            }

            return respuesta;
        }
        
        private static string EvaluateSensorDetailsAsync(Sensor unSensor)
        {
            if(unSensor.FrecuenciaMuestreo <=0)
                return "No se puede crear un sensor con frecuencia de muestreo menor o igual a cero.";
            
            if (string.IsNullOrEmpty(unSensor.FechaInstalacion))
                return "No se puede insertar una colmena con fecha de instalación nula";
            
            bool fechaValida = DateTime
                .TryParseExact(
                    unSensor.FechaInstalacion, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime fechaResultante);

            if (!fechaValida)
                return $"La fecha de instalación {unSensor.FechaInstalacion} no tiene el formato DD/MM/YYYY";

            if (fechaResultante > DateTime.Now)
                return $"No se puede registrar sensores con fecha de instalación futura. " +
                      $"La fecha actual es {DateTime.Now:dd/MM/yyyy} y la proporcionada es {fechaResultante:dd/MM/yyyy}";
            
            return string.Empty;
        }
    }
}