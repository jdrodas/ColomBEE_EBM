using ColomBEE_CSharp_NoSQL.API.Models;

namespace ColomBEE_CSharp_NoSQL.API.Interfaces;

public interface IEstadisticaRepository
{
    public Task<Estadistica> GetAllAsync();
}