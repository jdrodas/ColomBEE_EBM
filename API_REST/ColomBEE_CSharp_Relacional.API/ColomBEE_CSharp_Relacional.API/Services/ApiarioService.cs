using ColomBEE_CSharp_Relacional.API.Exceptions;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;

namespace ColomBEE_CSharp_Relacional.API.Services;

public class ApiarioService(IApiarioRepository apiarioRepository)
{
    private readonly IApiarioRepository _apiarioRepository = apiarioRepository;

    public async Task<List<Apiario>> GetAllAsync()
    {
        return await _apiarioRepository
            .GetAllAsync();
    }

    public async Task<Apiario> GetByIdAsync(Guid apiarioId)
    {
        var unApiario = await _apiarioRepository
            .GetByIdAsync(apiarioId);

        if (unApiario.Id == Guid.Empty)
            throw new EmptyCollectionException($"Apiario no encontrado con el Id {apiarioId}");

        return unApiario;
    }
    
    public async Task<List<Colmena>> GetAssociatedBeehivesAsync(Guid apiarioId)
    {
        var unApiario = await _apiarioRepository
            .GetByIdAsync(apiarioId);

        if (unApiario.Id == Guid.Empty)
            throw new EmptyCollectionException($"Apiario no encontrado con el Id {apiarioId}");

        var unasColmenasAsociadas = await _apiarioRepository
            .GetAssociatedBeehivesAsync(apiarioId);
        
        if(unasColmenasAsociadas.Count==0)
            throw new EmptyCollectionException($"Apiario {unApiario.Nombre} no tiene colmenas asociadas");
        
        return unasColmenasAsociadas;
    }

    public async Task<Apiario> CreateAsync(Apiario unApiario)
    {
        unApiario.Nombre = unApiario.Nombre!.Trim();

        var resultadoValidacion = EvaluateApiaryDetailsAsync(unApiario);

        if (!string.IsNullOrEmpty(resultadoValidacion))
            throw new AppValidationException(resultadoValidacion);

        var apiarioExistente = await _apiarioRepository
            .GetByDetailsAsync(unApiario);

        if (apiarioExistente.Nombre!.ToLower().Equals(unApiario.Nombre!.ToLower()) &&
            apiarioExistente.Latitud == unApiario.Latitud &&
            apiarioExistente.Longitud == unApiario.Longitud)
            throw new ConflictException($"No se puede crear el apiario {unApiario.Nombre} " +
                                        $"porque ya existe con id {apiarioExistente.Id}");

        var resultadoAccion = await _apiarioRepository
            .CreateAsync(unApiario);

        if (!resultadoAccion)
            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

        apiarioExistente = await _apiarioRepository
            .GetByDetailsAsync(unApiario);

        return apiarioExistente;
    }
    
    public async Task<Apiario> UpdateAsync(Apiario unApiario)
    {
        unApiario.Nombre = unApiario.Nombre!.Trim();

        var resultadoValidacion = EvaluateApiaryDetailsAsync(unApiario);

        if (!string.IsNullOrEmpty(resultadoValidacion))
            throw new AppValidationException(resultadoValidacion);

        var apiarioExistente = await _apiarioRepository
            .GetByIdAsync(unApiario.Id);
        
        if(apiarioExistente.Id == Guid.Empty)
            throw new EmptyCollectionException($"No existe un apiario con Id: {unApiario.Id} que se pueda actualizar");
        
        apiarioExistente = await _apiarioRepository
            .GetByDetailsAsync(unApiario);

        if (apiarioExistente.Nombre!.ToLower().Equals(unApiario.Nombre!.ToLower()) &&
            apiarioExistente.Latitud == unApiario.Latitud &&
            apiarioExistente.Longitud == unApiario.Longitud)
            throw new ConflictException($"No se puede actualizar el apiario {unApiario.Nombre} " +
                                        $"porque ya existe con id {apiarioExistente.Id}");

        var resultadoAccion = await _apiarioRepository
            .UpdateAsync(unApiario);

        if (!resultadoAccion)
            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

        apiarioExistente = await _apiarioRepository
            .GetByDetailsAsync(unApiario);

        return apiarioExistente;
    }

    public async Task<string> RemoveAsync(Guid apiarioId)
    {
        var respuesta = "resultado";

        var apiarioExistente = await _apiarioRepository
            .GetByIdAsync(apiarioId);

        if (apiarioExistente.Id == Guid.Empty)
            throw new EmptyCollectionException($"No hay un apiario con id {apiarioId}");

        var totalColmenasAsociadas = await _apiarioRepository
            .GetTotalAssociatedBeehivesAsync(apiarioId);

        if (totalColmenasAsociadas > 0)
            throw new AppValidationException(
                $"El apiario con id {apiarioId} tiene {totalColmenasAsociadas} colmenas asociadas. No se puede eliminar");

        var resultado = await _apiarioRepository
            .RemoveAsync(apiarioId);

        if (resultado)
            respuesta = $"Eliminado el apiario {apiarioExistente.Nombre}.";

        return respuesta;
    }

    private static string EvaluateApiaryDetailsAsync(Apiario unApiario)
    {
        if (string.IsNullOrEmpty(unApiario.Nombre))
            return "No se puede insertar un apiario con nombre nulo";

        if (unApiario.Latitud > 90 || unApiario.Latitud < -90)
            return "La latitud de la coordenada geográfica del apiario debe ser un valor entre [-90;90]";

        if (unApiario.Longitud > 180 || unApiario.Longitud < -180)
            return "La longitud de la coordenada geográfica del apiario debe ser un valor entre [-180;180]";

        return string.Empty;
    }
}