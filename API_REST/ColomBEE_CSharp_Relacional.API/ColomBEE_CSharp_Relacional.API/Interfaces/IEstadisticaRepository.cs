using ColomBEE_CSharp_Relacional.API.Models;

namespace ColomBEE_CSharp_Relacional.API.Interfaces
{
    public interface IEstadisticaRepository
    {
        public Task<Estadistica> GetAllAsync();
    }
}