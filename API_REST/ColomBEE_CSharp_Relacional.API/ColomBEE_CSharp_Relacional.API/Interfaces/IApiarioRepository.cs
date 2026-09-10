using ColomBEE_CSharp_Relacional.API.Models;

namespace ColomBEE_CSharp_Relacional.API.Interfaces
{
    public interface IApiarioRepository
    {
        public Task<List<Apiario>> GetAllAsync();
        public Task<Apiario> GetByIdAsync(Guid apiarioId);
    }
}

