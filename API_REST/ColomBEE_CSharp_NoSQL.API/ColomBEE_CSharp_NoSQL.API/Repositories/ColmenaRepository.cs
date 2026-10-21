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
        catch (TimeoutException)
        {
            throw new DbOperationException("Se ha cumplido el tiempo de espera para la conexión a la base de datos");
        }
    }
    public async Task<Colmena> GetByDetailsAsync(Colmena unaColmena)
    {
        try
        {
            Colmena colmenaExistente = new();

            var conexion = _contextoDb
                .CreateConnection();

            var coleccionColmenas = conexion
                .GetCollection<Colmena>(_contextoDb.ConfiguracionColecciones.ColeccionColmenas);
            
            var builder = Builders<Colmena>.Filter;
            var filtro = builder.And(
                builder.Regex(colmena => colmena.Nombre, $"/^{unaColmena.Nombre}$/i"),
                builder.Regex(colmena => colmena.FechaInstalacion, $"/^{unaColmena.FechaInstalacion}$/i"),
                builder.Eq(colmena => colmena.ApiarioId, unaColmena.ApiarioId)
            );

            var resultado = await coleccionColmenas
                .Find(filtro)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                colmenaExistente = resultado;

            return colmenaExistente;
        }
        catch (TimeoutException)
        {
            throw new DbOperationException("Se ha cumplido el tiempo de espera para la conexión a la base de datos");
        }
    }
    
    public async Task<List<Sensor>> GetAssociatedBeehivesAsync(string colmenaÏd)
    {
        if (!ObjectId.TryParse(colmenaÏd, out var colmena_objectId))
            throw new FormatException($"El valor '{colmenaÏd}' no tiene un formato válido de ObjectId.");
        
        var conexion = _contextoDb
            .CreateConnection();

        var coleccionSensores = conexion
            .GetCollection<Sensor>(_contextoDb.ConfiguracionColecciones.ColeccionSensores);

        var losSensores = await coleccionSensores
            .Find(sensor => sensor.ColmenaId == colmenaÏd)
            .ToListAsync();

        return losSensores;
    }
    
    public async Task<long> GetTotalAssociatedSensorsAsync(string colmenaÏd)
    {
        if (!ObjectId.TryParse(colmenaÏd, out var colmena_objectId))
            throw new FormatException($"El valor '{colmenaÏd}' no tiene un formato válido de ObjectId.");

        var losSensores = await GetAssociatedBeehivesAsync(colmenaÏd);

        if (losSensores.Any())
            return losSensores.Count;
        else
            return 0;
    }

    
    public async Task<bool> CreateAsync(Colmena unaColmena)
    {
        bool resultadoAccion = false;

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionColmenas = conexion
            .GetCollection<Colmena>(_contextoDb.ConfiguracionColecciones.ColeccionColmenas);

        await coleccionColmenas
            .InsertOneAsync(unaColmena);

        var resultado = await GetByDetailsAsync(unaColmena);

        if (resultado is not null)
            resultadoAccion = true;

        return resultadoAccion;
    }
    public async Task<bool> RemoveAsync(string colmenaId)
    {
        if (!ObjectId.TryParse(colmenaId, out var colmena_objectId))
            throw new FormatException($"El valor '{colmenaId}' no tiene un formato válido de ObjectId.");
        
        bool resultadoAccion = false;

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionColmenas = conexion
            .GetCollection<Colmena>(_contextoDb.ConfiguracionColecciones.ColeccionColmenas);

        var resultado = await coleccionColmenas
            .DeleteOneAsync(colmena => colmena.Id == colmenaId);

        if (resultado.IsAcknowledged)
            resultadoAccion = true;

        return resultadoAccion;
    }
}