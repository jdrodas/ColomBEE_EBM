using ColomBEE_CSharp_Relacional.API.Models;

namespace ColomBEE_CSharp_Relacional.API.Interfaces
{
    public interface IColmenaRepository
    {
        public Task<List<Colmena>> GetAllAsync();
        public Task<Colmena> GetByIdAsync(Guid colmenaId);
        public Task<Colmena> GetByDetailsAsync(Colmena unaColmena);
        public Task<bool> CreateAsync(Colmena unaColmena);
    }
}