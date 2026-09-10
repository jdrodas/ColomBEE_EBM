using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Exceptions;

namespace ColomBEE_CSharp_Relacional.API.Services
{

    public class ColmenaService(IColmenaRepository colmenaRepository)
    {
        private readonly IColmenaRepository _colmenaRepository = colmenaRepository;

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
    }
}