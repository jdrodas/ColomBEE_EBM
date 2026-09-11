using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using System.Globalization;

namespace ColomBEE_CSharp_Relacional.API.Services
{

    public class ColmenaService(IColmenaRepository colmenaRepository,
                                IApiarioRepository apiarioRepository)
    {
        private readonly IColmenaRepository _colmenaRepository = colmenaRepository;
        private readonly IApiarioRepository _apiarioRepository = apiarioRepository;
        
        public async Task<List<Colmena>> GetAllAsync()
        {
            return await _colmenaRepository
                .GetAllAsync();
        }
        
        public async Task<Colmena> GetByIdAsync(Guid colmenaId)
        {
            Colmena unaColmena = await _colmenaRepository
                .GetByIdAsync(colmenaId);

            if (unaColmena.Id == Guid.Empty)
                throw new EmptyCollectionException($"Colmena no encontrada con el Id {colmenaId}");

            return unaColmena;
        }
        
        public async Task<Colmena> CreateAsync(Colmena unaColmena)
        {
            unaColmena.Codigo = unaColmena.Codigo!.Trim();
            unaColmena.FechaInstalacion = unaColmena.FechaInstalacion!.Trim();

            string resultadoValidacion = EvaluateBeehiveDetailsAsync(unaColmena);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var apiarioExistente = await _apiarioRepository
                .GetByIdAsync(unaColmena.ApiarioId);
            
            if (apiarioExistente.Id == Guid.Empty)
                throw new AppValidationException("No existe apiario con Id {unaColmena.ApiarioId}");

            unaColmena.ApiarioNombre = apiarioExistente.Nombre;
            
            var colmenaExistente = await _colmenaRepository
                .GetByDetailsAsync(unaColmena);

            if (colmenaExistente.Id != Guid.Empty)
                return colmenaExistente;
            
            try
            {
                bool resultadoAccion = await _colmenaRepository
                    .CreateAsync(unaColmena);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                colmenaExistente = await _colmenaRepository
                    .GetByDetailsAsync(unaColmena);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return colmenaExistente;
        }
        
        private static string EvaluateBeehiveDetailsAsync(Colmena unaColmena)
        {
            if (string.IsNullOrEmpty(unaColmena.Codigo))
                return "No se puede insertar una colmena con codigo nulo";

            if (string.IsNullOrEmpty(unaColmena.FechaInstalacion))
                return "No se puede insertar una colmena con fecha de instalación nula";
            
            bool fechaValida = DateTime
                .TryParseExact(
                    unaColmena.FechaInstalacion, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime fechaResultante);

            if (!fechaValida)
                throw new AppValidationException($"La fecha de instalación {unaColmena.FechaInstalacion} no tiene el formato DD/MM/YYYY");

            if (fechaResultante >= DateTime.Now)
                throw new AppValidationException($"No se puede registrar colmenas con fecha de instalación futura. " +
                                                 $"La fecha actual es {DateTime.Now:DD/MM/YYYY}");
            
            return string.Empty;
        }

    }
}