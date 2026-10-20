using System.Data;
using ColomBEE_CSharp_NoSQL.API.DbContexts;
using ColomBEE_CSharp_NoSQL.API.Exceptions;
using ColomBEE_CSharp_NoSQL.API.Interfaces;
using ColomBEE_CSharp_NoSQL.API.Models;
using MongoDB.Driver; 
using MongoDB.Bson;

namespace ColomBEE_CSharp_NoSQL.API.Repositories;

public class SensorRepository(MongoDbContext unContexto) : ISensorRepository
{
    private readonly MongoDbContext _contextoDb = unContexto;

    public async Task<List<Sensor>> GetAllAsync()
    {
        var conexion = _contextoDb
            .CreateConnection();

        var coleccionSensores = conexion
            .GetCollection<Sensor>(_contextoDb.ConfiguracionColecciones.ColeccionSensores);

        var losSensores = await coleccionSensores
            .Find(_ => true)
            .SortBy(sensor => sensor.TipoSensorNombre)
            .ToListAsync();

        return losSensores;
    }
    
    public async Task<Sensor> GetByIdAsync(string sensorId)
    {
        if (!ObjectId.TryParse(sensorId, out var sensor_objectId))
            throw new FormatException($"El valor '{sensorId}' no tiene un formato válido de ObjectId.");
        
        Sensor unSensor = new();

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionSensores = conexion
            .GetCollection<Sensor>(_contextoDb.ConfiguracionColecciones.ColeccionSensores);

        var resultado = await coleccionSensores
            .Find(sensor => sensor.Id == sensorId)
            .FirstOrDefaultAsync();

        if (resultado is not null)
            unSensor = resultado;

        return unSensor;
    }
}