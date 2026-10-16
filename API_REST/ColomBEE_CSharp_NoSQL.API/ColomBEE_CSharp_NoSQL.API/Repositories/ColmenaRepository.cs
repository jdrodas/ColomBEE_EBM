using System.Data;
using ColomBEE_CSharp_NoSQL.API.DbContexts;
using ColomBEE_CSharp_NoSQL.API.Exceptions;
using ColomBEE_CSharp_NoSQL.API.Interfaces;
using ColomBEE_CSharp_NoSQL.API.Models;
using MongoDB.Driver; 
using MongoDB.Bson;


namespace ColomBEE_CSharp_NoSQL.API.Repositories;

public class ColmenaRepository(MongoDbContext unContexto) : IColmenaRepository
{
    private readonly MongoDbContext _contextoDb = unContexto;

    public async Task<List<Colmena>> GetAllAsync()
    {
        var conexion = _contextoDb
            .CreateConnection();

        var coleccionColmenas = conexion
            .GetCollection<Colmena>(_contextoDb.ConfiguracionColecciones.ColeccionColmenas);

        var lasColmenas = await coleccionColmenas
            .Find(_ => true)
            .SortBy(colmena => colmena.Nombre)
            .ToListAsync();

        return lasColmenas;
    }
    
    public async Task<Colmena> GetByIdAsync(string colmenaId)
    {
        if (!ObjectId.TryParse(colmenaId, out var apiario_objectId))
            throw new FormatException($"El valor '{colmenaId}' no tiene un formato válido de ObjectId.");

        try
        {
            Colmena unaColmena = new();

            var conexion = _contextoDb
                .CreateConnection();

            var coleccionColmenas = conexion
                .GetCollection<Colmena>(_contextoDb.ConfiguracionColecciones.ColeccionColmenas);

            var resultado = await coleccionColmenas
                .Find(colmena => colmena.Id == colmenaId)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaColmena = resultado;

            return unaColmena;
        }
        catch (TimeoutException error)
        {
            throw new DbOperationException("Se ha cumplido el tiempo de espera para la conexión a la base de datos");
        }
    }
}