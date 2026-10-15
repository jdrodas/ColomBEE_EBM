using ColomBEE_CSharp_NoSQL.API.Interfaces;
using ColomBEE_CSharp_NoSQL.API.Models;

namespace ColomBEE_CSharp_NoSQL.API.Services;

public class EstadisticaService(IEstadisticaRepository estadisticaRepository)
{
    private readonly IEstadisticaRepository _estadisticaRepository = estadisticaRepository;

    public async Task<Estadistica> GetAllAsync()
    {
        return await _estadisticaRepository
            .GetAllAsync();
    }
}