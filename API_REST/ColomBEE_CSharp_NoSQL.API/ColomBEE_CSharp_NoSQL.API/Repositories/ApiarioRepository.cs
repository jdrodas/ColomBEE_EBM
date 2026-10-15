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
}