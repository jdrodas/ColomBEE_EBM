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
    }
}