using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Exceptions;

namespace ColomBEE_CSharp_Relacional.API.Services
{

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
            Apiario unApiario = await _apiarioRepository
                .GetByIdAsync(apiarioId);

            if (unApiario.Id == Guid.Empty)
                throw new EmptyCollectionException($"Apiario no encontrado con el Id {apiarioId}");

            return unApiario;
        }
        
        public async Task<Apiario> CreateAsync(Apiario unApiario)
        {
            unApiario.Nombre = unApiario.Nombre!.Trim();

            string resultadoValidacion = EvaluateApiaryDetailsAsync(unApiario);

            if (!string.IsNullOrEmpty(resultadoValidacion))
                throw new AppValidationException(resultadoValidacion);

            var apiarioExistente = await _apiarioRepository
                .GetByDetailsAsync(unApiario);

            if (apiarioExistente.Equals(unApiario))
                return apiarioExistente;

            try
            {
                bool resultadoAccion = await _apiarioRepository
                    .CreateAsync(unApiario);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                apiarioExistente = await _apiarioRepository
                    .GetByDetailsAsync(unApiario);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return apiarioExistente;
        }
        
        private static string EvaluateApiaryDetailsAsync(Apiario unApiario)
        {
            if (string.IsNullOrEmpty(unApiario.Nombre))
                return "No se puede insertar un apiario con nombre nulo";

            if(unApiario.Latitud > 90 || unApiario.Latitud <-90)
                return "La latitud de la coordenada geográfica del apiario debe ser un valor entre [-90;90]";

            if(unApiario.Longitud > 180 || unApiario.Longitud <-180)
                return "La longitud de la coordenada geográfica del apiario debe ser un valor entre [-180;180]";
            
            return string.Empty;
        }
    }
}