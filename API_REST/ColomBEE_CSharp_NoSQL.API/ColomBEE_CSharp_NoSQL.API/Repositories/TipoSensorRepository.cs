using System.Data;
using ColomBEE_CSharp_NoSQL.API.DbContexts;
using ColomBEE_CSharp_NoSQL.API.Exceptions;
using ColomBEE_CSharp_NoSQL.API.Interfaces;
using ColomBEE_CSharp_NoSQL.API.Models;
using MongoDB.Driver; 
using MongoDB.Bson;

namespace ColomBEE_CSharp_NoSQL.API.Repositories;

public class TipoSensorRepository(MongoDbContext unContexto) : ITipoSensorRepository
{
    private readonly MongoDbContext _contextoDb = unContexto;

    public async Task<List<TipoSensor>> GetAllAsync()
    {
        var conexion = _contextoDb
            .CreateConnection();

        var coleccionTiposSensores = conexion
            .GetCollection<TipoSensor>(_contextoDb.ConfiguracionColecciones.ColeccionTiposSensores);

        var losTiposSensores = await coleccionTiposSensores
            .Find(_ => true)
            .SortBy(tipoSensor => tipoSensor.Nombre)
            .ToListAsync();

        return losTiposSensores;
    }
    
    public async Task<TipoSensor> GetByIdAsync(string tipoSensorId)
    {
        if (!ObjectId.TryParse(tipoSensorId, out var tipo_sensor_objectId))
            throw new FormatException($"El valor '{tipoSensorId}' no tiene un formato válido de ObjectId.");
        
        TipoSensor unTipoSensor = new();

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionTiposSensores = conexion
            .GetCollection<TipoSensor>(_contextoDb.ConfiguracionColecciones.ColeccionTiposSensores);

        var resultado = await coleccionTiposSensores
            .Find(tipoSensor => tipoSensor.Id == tipoSensorId)
            .FirstOrDefaultAsync();

        if (resultado is not null)
            unTipoSensor = resultado;

        return unTipoSensor;
    }
}