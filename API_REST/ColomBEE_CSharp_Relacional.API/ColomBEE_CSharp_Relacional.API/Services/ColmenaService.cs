using System.Globalization;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;

namespace ColomBEE_CSharp_Relacional.API.Services;

public class ColmenaService(
    IColmenaRepository colmenaRepository,
    IApiarioRepository apiarioRepository)
{
    private readonly IApiarioRepository _apiarioRepository = apiarioRepository;
    private readonly IColmenaRepository _colmenaRepository = colmenaRepository;

    public async Task<List<Colmena>> GetAllAsync()
    {
        return await _colmenaRepository
            .GetAllAsync();
    }

    public async Task<Colmena> GetByIdAsync(Guid colmenaId)
    {
        var unaColmena = await _colmenaRepository
            .GetByIdAsync(colmenaId);

        if (unaColmena.Id == Guid.Empty)
            throw new EmptyCollectionException($"Colmena no encontrada con el Id {colmenaId}");

        return unaColmena;
    }

    public async Task<Colmena> CreateAsync(Colmena unaColmena)
    {
        unaColmena.Codigo = unaColmena.Codigo!.Trim();
        unaColmena.FechaInstalacion = unaColmena.FechaInstalacion!.Trim();

        var resultadoValidacion = EvaluateBeehiveDetailsAsync(unaColmena);

        if (!string.IsNullOrEmpty(resultadoValidacion))
            throw new AppValidationException(resultadoValidacion);

        var apiarioExistente = await _apiarioRepository
            .GetByIdAsync(unaColmena.ApiarioId);

        if (apiarioExistente.Id == Guid.Empty)
            throw new AppValidationException($"No existe apiario con Id {unaColmena.ApiarioId}");

        unaColmena.ApiarioNombre = apiarioExistente.Nombre;

        var colmenaExistente = await _colmenaRepository
            .GetByDetailsAsync(unaColmena);

        if (colmenaExistente.Id != Guid.Empty)
            throw new ConflictException($"No se puede crear la colmena {unaColmena.Codigo} " +
                                        $"porque ya existe con id {colmenaExistente.Id}");

        var resultadoAccion = await _colmenaRepository
            .CreateAsync(unaColmena);

        if (!resultadoAccion)
            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

        colmenaExistente = await _colmenaRepository
            .GetByDetailsAsync(unaColmena);

        return colmenaExistente;
    }
    
    public async Task<Colmena> UpdateAsync(Colmena unaColmena)
    {
        unaColmena.Codigo = unaColmena.Codigo!.Trim();
        unaColmena.FechaInstalacion = unaColmena.FechaInstalacion!.Trim();

        var resultadoValidacion = EvaluateBeehiveDetailsAsync(unaColmena);

        if (!string.IsNullOrEmpty(resultadoValidacion))
            throw new AppValidationException(resultadoValidacion);
        
        var apiarioExistente = await _apiarioRepository
            .GetByIdAsync(unaColmena.ApiarioId);

        if (apiarioExistente.Id == Guid.Empty)
            throw new AppValidationException($"No existe apiario con Id {unaColmena.ApiarioId}");

        unaColmena.ApiarioNombre = apiarioExistente.Nombre;

        var colmenaExistente = await _colmenaRepository
            .GetByIdAsync(unaColmena.Id);
        
        if(colmenaExistente.Id == Guid.Empty)
            throw new EmptyCollectionException($"No existe una colmena con Id: {unaColmena.Id} " +
                                               $"que se pueda actualizar");
        
        colmenaExistente = await _colmenaRepository
            .GetByDetailsAsync(unaColmena);

        if (colmenaExistente.Codigo!.ToLower().Equals(unaColmena.Codigo!.ToLower()) &&
            colmenaExistente.ApiarioId == unaColmena.ApiarioId &&
            colmenaExistente.FechaInstalacion == unaColmena.FechaInstalacion)
            throw new ConflictException($"No se puede actualizar la Colmena {unaColmena.Codigo} " +
                                        $"porque ya existe con id {colmenaExistente.Id}");

        var resultadoAccion = await _colmenaRepository
            .UpdateAsync(unaColmena);

        if (!resultadoAccion)
            throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

        colmenaExistente = await _colmenaRepository
            .GetByDetailsAsync(unaColmena);

        return colmenaExistente;
    }
    
    public async Task<string> RemoveAsync(Guid colmenaId)
    {
        var respuesta = "resultado";

        var colmenaExistente = await _colmenaRepository
            .GetByIdAsync(colmenaId);

        if (colmenaExistente.Id == Guid.Empty)
            throw new EmptyCollectionException($"No hay una colmena con id {colmenaId}");

        var totalSensoresAsociados = await _colmenaRepository
            .GetTotalAssociatedSensorsAsync(colmenaId);

        if (totalSensoresAsociados > 0)
            throw new AppValidationException(
                $"La colmena con id {colmenaId} tiene {totalSensoresAsociados} sensores asociados. " +
                $"No se puede eliminar");

        var resultado = await _colmenaRepository
            .RemoveAsync(colmenaId);

        if (resultado)
            respuesta = $"Eliminada la colmena {colmenaExistente.Codigo} " +
                        $"asociada al apiario {colmenaExistente.ApiarioNombre}.";

        return respuesta;
    }

    private static string EvaluateBeehiveDetailsAsync(Colmena unaColmena)
    {
        if (string.IsNullOrEmpty(unaColmena.Codigo))
            return "No se puede insertar una colmena con codigo nulo";

        if (string.IsNullOrEmpty(unaColmena.FechaInstalacion))
            return "No se puede insertar una colmena con fecha de instalación nula";

        var fechaValida = DateTime
            .TryParseExact(
                unaColmena.FechaInstalacion, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var fechaResultante);

        if (!fechaValida)
            return $"La fecha de instalación {unaColmena.FechaInstalacion} no tiene el formato DD/MM/YYYY";

        if (fechaResultante > DateTime.Now)
            return $"No se puede registrar colmenas con fecha de instalación futura. " +
                   $"La fecha actual es {DateTime.Now:dd/MM/yyyy}";

        return string.Empty;
    }
}