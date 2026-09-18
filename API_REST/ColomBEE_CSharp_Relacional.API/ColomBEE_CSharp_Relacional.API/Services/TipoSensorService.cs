using ColomBEE_CSharp_Relacional.API.Exceptions;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;

namespace ColomBEE_CSharp_Relacional.API.Services;

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
        var unTipoSensor = await _tipoSensorRepository
            .GetByIdAsync(tipoSensorId);

        if (unTipoSensor.Id == Guid.Empty)
            throw new EmptyCollectionException($"Tipo Sensor no encontrado con el Id {tipoSensorId}");

        return unTipoSensor;
    }
    
    public async Task<List<Sensor>> GetAssociatedSensorsAsync(Guid tipoSensorId)
    {
        var unTipoSensor = await _tipoSensorRepository
            .GetByIdAsync(tipoSensorId);

        if (unTipoSensor.Id == Guid.Empty)
            throw new EmptyCollectionException($"Tipo de sensor no encontrado con el Id {tipoSensorId}");

        var unosSensoresAsociados = await _tipoSensorRepository
            .GetAssociatedSensorsAsync(tipoSensorId);
        
        if(unosSensoresAsociados.Count==0)
            throw new EmptyCollectionException($"El tipo de sensor {unTipoSensor.Nombre} no tiene sensores asociados");
        
        return unosSensoresAsociados;
    }

    public async Task<TipoSensor> CreateAsync(TipoSensor unTipoSensor)
    {
        unTipoSensor.Nombre = unTipoSensor.Nombre!.Trim();
        unTipoSensor.UnidadMedida = unTipoSensor.UnidadMedida!.Trim();

        var resultadoValidacion = EvaluateSensorTypeDetailsAsync(unTipoSensor);

        if (!string.IsNullOrEmpty(resultadoValidacion))
            throw new AppValidationException(resultadoValidacion);

        var tipoSensorExistente = await _tipoSensorRepository
            .GetByDetailsAsync(unTipoSensor);

        if (tipoSensorExistente.Nombre!.ToUpper().Equals(unTipoSensor.Nombre.ToUpper()) &&
            tipoSensorExistente.UnidadMedida!.ToUpper().Equals(unTipoSensor.UnidadMedida.ToUpper()))
            throw new ConflictException($"No se puede crear el tipo de sensor {unTipoSensor.Nombre} " +
                                        $"porque ya existe con id {tipoSensorExistente.Id}");

        var resultadoAccion = await _tipoSensorRepository
            .CreateAsync(unTipoSensor);

        if (!resultadoAccion)
            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

        tipoSensorExistente = await _tipoSensorRepository
            .GetByDetailsAsync(unTipoSensor);

        return tipoSensorExistente;
    }
    
    public async Task<TipoSensor> UpdateAsync(TipoSensor unTipoSensor)
    {
        unTipoSensor.Nombre = unTipoSensor.Nombre!.Trim();
        unTipoSensor.UnidadMedida = unTipoSensor.UnidadMedida!.Trim();

        var resultadoValidacion = EvaluateSensorTypeDetailsAsync(unTipoSensor);

        if (!string.IsNullOrEmpty(resultadoValidacion))
            throw new AppValidationException(resultadoValidacion);

        var tipoSensorExistente = await _tipoSensorRepository
            .GetByIdAsync(unTipoSensor.Id);
        
        if(tipoSensorExistente.Id == Guid.Empty)
            throw new EmptyCollectionException($"No existe un tipo de sensor con Id: {unTipoSensor.Id} " +
                                               $"que se pueda actualizar");
        
        tipoSensorExistente = await _tipoSensorRepository
            .GetByDetailsAsync(unTipoSensor);

        if (tipoSensorExistente.Nombre!.ToLower().Equals(unTipoSensor.Nombre!.ToLower()) &&
            tipoSensorExistente.UnidadMedida == unTipoSensor.UnidadMedida)
            throw new ConflictException($"No se puede actualizar el tipo de sensor {unTipoSensor.Nombre} " +
                                        $"porque ya existe con id {tipoSensorExistente.Id}");

        var resultadoAccion = await _tipoSensorRepository
            .UpdateAsync(unTipoSensor);

        if (!resultadoAccion)
            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

        tipoSensorExistente = await _tipoSensorRepository
            .GetByDetailsAsync(unTipoSensor);

        return tipoSensorExistente;
    }

    public async Task<string> RemoveAsync(Guid tipoSensorId)
    {
        var respuesta = "resultado";

        var tipoSensorExistente = await _tipoSensorRepository
            .GetByIdAsync(tipoSensorId);

        if (tipoSensorExistente.Id == Guid.Empty)
            throw new EmptyCollectionException($"No hay un tipo de sensor con id {tipoSensorId}");

        var totalSensoresAsociados = await _tipoSensorRepository
            .GetTotalAssociatedSensorsAsync(tipoSensorId);

        if (totalSensoresAsociados > 0)
            throw new AppValidationException(
                $"El tipo de sensor con id {tipoSensorId} tiene {totalSensoresAsociados} sensores asociados. No se puede eliminar");


        var resultado = await _tipoSensorRepository
            .RemoveAsync(tipoSensorId);

        if (resultado)
            respuesta =
                $"Eliminado el tipo de sensor {tipoSensorExistente.Nombre} con unidad de medida {tipoSensorExistente.UnidadMedida}";

        return respuesta;
    }

    private static string EvaluateSensorTypeDetailsAsync(TipoSensor unTipoSensor)
    {
        if (string.IsNullOrEmpty(unTipoSensor.Nombre))
            return "No se puede insertar o actualizar un tipo de sensor con nombre nulo";

        if (string.IsNullOrEmpty(unTipoSensor.UnidadMedida))
            return "No se puede insertar o actualizar un tipo de sensor con unidad de medida nula";

        return string.Empty;
    }
}