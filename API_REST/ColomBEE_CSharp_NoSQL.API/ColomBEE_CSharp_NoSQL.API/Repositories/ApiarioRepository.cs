using System.Data;
using ColomBEE_CSharp_NoSQL.API.DbContexts;
using ColomBEE_CSharp_NoSQL.API.Exceptions;
using ColomBEE_CSharp_NoSQL.API.Interfaces;
using ColomBEE_CSharp_NoSQL.API.Models;
using MongoDB.Driver; 
using MongoDB.Bson;

namespace ColomBEE_CSharp_NoSQL.API.Repositories;

public class ApiarioRepository(MongoDbContext unContexto) :  IApiarioRepository
{
    private readonly MongoDbContext _contextoDb = unContexto;

    public async Task<List<Apiario>> GetAllAsync()
    {
        var conexion = _contextoDb
            .CreateConnection();

        var coleccionApiarios = conexion
            .GetCollection<Apiario>(_contextoDb.ConfiguracionColecciones.ColeccionApiarios);

        var losApiarios = await coleccionApiarios
            .Find(_ => true)
            .SortBy(apiario => apiario.Nombre)
            .ToListAsync();

        return losApiarios;
    }
    
    public async Task<Apiario> GetByIdAsync(string apiarioId)
    {
        if (!ObjectId.TryParse(apiarioId, out var apiario_objectId))
            throw new FormatException($"El valor '{apiarioId}' no tiene un formato válido de ObjectId.");
        
        Apiario unApiario = new();

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionApiarios = conexion
            .GetCollection<Apiario>(_contextoDb.ConfiguracionColecciones.ColeccionApiarios);

        var resultado = await coleccionApiarios
            .Find(apiario => apiario.Id == apiarioId)
            .FirstOrDefaultAsync();

        if (resultado is not null)
            unApiario = resultado;

        return unApiario;
    }
    
    public async Task<long> GetTotalAssociatedBeehivesAsync(string apiarioId)
    {
        if (!ObjectId.TryParse(apiarioId, out var apiario_objectId))
            throw new FormatException($"El valor '{apiarioId}' no tiene un formato válido de ObjectId.");
        
        var conexion = _contextoDb
            .CreateConnection();

        var coleccionColmenas = conexion
            .GetCollection<Colmena>(_contextoDb.ConfiguracionColecciones.ColeccionColmenas);

        var resultado = await coleccionColmenas
            .Find(colemna => colemna.ApiarioId == apiarioId)
            .FirstOrDefaultAsync();
        
        var lasColmenas = await coleccionColmenas
            .Find(colmena => colmena.ApiarioId == apiarioId)
            .ToListAsync();

        if (lasColmenas.Any())
            return lasColmenas.Count;
        else
            return 0;
    }
    
    public async Task<Apiario> GetByNameAsync(string apiarioNombre)
    {
        Apiario apiarioExistente = new();

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionApiarios = conexion
            .GetCollection<Apiario>(_contextoDb.ConfiguracionColecciones.ColeccionApiarios);

        var builder = Builders<Apiario>.Filter;
        var filtro = builder
            .Regex(apiario => apiario.Nombre, $"/^{apiarioNombre}$/i");

        var resultado = await coleccionApiarios
            .Find(filtro)
            .FirstOrDefaultAsync();

        if (resultado is not null)
            apiarioExistente = resultado;

        return apiarioExistente;
    }
    
    public async Task<Apiario> GetByDetailsAsync(Apiario unApiario)
    {
        Apiario apiarioExistente = new();

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionApiarios = conexion
            .GetCollection<Apiario>(_contextoDb.ConfiguracionColecciones.ColeccionApiarios);

        var builder = Builders<Apiario>.Filter;
        var filtro = builder.And(
            builder.Regex(apiario => apiario.Nombre, $"/^{unApiario.Nombre}$/i"),
            builder.Eq(apiario => apiario.Latitud, unApiario.Latitud),
            builder.Eq(apiario => apiario.Longitud, unApiario.Longitud)
        );

        var resultado = await coleccionApiarios
            .Find(filtro)
            .FirstOrDefaultAsync();

        if (resultado is not null)
            apiarioExistente = resultado;

        return apiarioExistente;
    }
    
    public async Task<bool> CreateAsync(Apiario unApiario)
    {
        bool resultadoAccion = false;

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionApiarios = conexion
            .GetCollection<Apiario>(_contextoDb.ConfiguracionColecciones.ColeccionApiarios);

        await coleccionApiarios
            .InsertOneAsync(unApiario);

        var resultado = await GetByDetailsAsync(unApiario);

        if (resultado is not null)
            resultadoAccion = true;

        return resultadoAccion;
    }
    
    public async Task<bool> RemoveAsync(string apiarioId)
    {
        if (!ObjectId.TryParse(apiarioId, out var apiario_objectId))
            throw new FormatException($"El valor '{apiarioId}' no tiene un formato válido de ObjectId.");
        
        bool resultadoAccion = false;

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionApiarios = conexion
            .GetCollection<Apiario>(_contextoDb.ConfiguracionColecciones.ColeccionApiarios);

        var resultado = await coleccionApiarios
            .DeleteOneAsync(apiario => apiario.Id == apiarioId);

        if (resultado.IsAcknowledged)
            resultadoAccion = true;

        return resultadoAccion;
    }
}