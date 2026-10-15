using ColomBEE_CSharp_NoSQL.API.DbContexts;
using ColomBEE_CSharp_NoSQL.API.Interfaces;
using ColomBEE_CSharp_NoSQL.API.Models;

namespace ColomBEE_CSharp_NoSQL.API.Repositories;

public class EstadisticaRepository(MongoDbContext unContexto) : IEstadisticaRepository
{
    private readonly MongoDbContext contextoDB = unContexto;

    public async Task<Estadistica> GetAllAsync()
    {
        Estadistica unaEstadistica = new();

        var conexion = contextoDB
            .CreateConnection();

        var coleccionApiarios = conexion
            .GetCollection<Apiario>(contextoDB.ConfiguracionColecciones.ColeccionApiarios);

        var totalApiarios = await coleccionApiarios
            .EstimatedDocumentCountAsync();

        unaEstadistica.Apiarios = totalApiarios;

        return unaEstadistica;
    }
}