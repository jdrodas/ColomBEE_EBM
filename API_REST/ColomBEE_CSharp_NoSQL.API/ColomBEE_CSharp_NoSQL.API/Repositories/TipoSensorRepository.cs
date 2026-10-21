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
    
    public async Task<TipoSensor> GetByDetailsAsync(TipoSensor unTipoSensor)
    {
        TipoSensor tipoSensorExistente = new();

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionTiposSensores = conexion
            .GetCollection<TipoSensor>(_contextoDb.ConfiguracionColecciones.ColeccionTiposSensores);

        var builder = Builders<TipoSensor>.Filter;
        var filtro = builder.And(
            builder.Regex(tipoSensor => tipoSensor.Nombre, $"/^{unTipoSensor.Nombre}$/i"),
            builder.Regex(tipoSensor => tipoSensor.UnidadMedida, $"/^{unTipoSensor.UnidadMedida}$/i")
        );

        var resultado = await coleccionTiposSensores
            .Find(filtro)
            .FirstOrDefaultAsync();

        if (resultado is not null)
            tipoSensorExistente = resultado;

        return tipoSensorExistente;
    }
    
    public async Task<List<Sensor>> GetAssociatedSensorsAsync(string tipoSensorId)
    {
        if (!ObjectId.TryParse(tipoSensorId, out var tipoSensor_objectId))
            throw new FormatException($"El valor '{tipoSensorId}' no tiene un formato válido de ObjectId.");
        
        var conexion = _contextoDb
            .CreateConnection();

        var coleccionSensores = conexion
            .GetCollection<Sensor>(_contextoDb.ConfiguracionColecciones.ColeccionSensores);

        var losSensores = await coleccionSensores
            .Find(sensor => sensor.TipoId == tipoSensorId)
            .ToListAsync();

        return losSensores;
    }
    
    public async Task<long> GetTotalAssociatedSensorsAsync(string tipoSensorId)
    {
        if (!ObjectId.TryParse(tipoSensorId, out var tipoSensor_objectId))
            throw new FormatException($"El valor '{tipoSensorId}' no tiene un formato válido de ObjectId.");

        var losSensores = await GetAssociatedSensorsAsync(tipoSensorId);

        if (losSensores.Any())
            return losSensores.Count;
        else
            return 0;
    }
    
    public async Task<bool> CreateAsync(TipoSensor unTipoSensor)
    {
        bool resultadoAccion = false;

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionTiposSensores = conexion
            .GetCollection<TipoSensor>(_contextoDb.ConfiguracionColecciones.ColeccionTiposSensores);

        await coleccionTiposSensores
            .InsertOneAsync(unTipoSensor);

        var resultado = await GetByDetailsAsync(unTipoSensor);

        if (resultado is not null)
            resultadoAccion = true;

        return resultadoAccion;
    }
    
    public async Task<bool> UpdateAsync(TipoSensor unTipoSensor)
    {
        bool resultadoAccion = false;

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionTiposSensores = conexion
            .GetCollection<TipoSensor>(_contextoDb.ConfiguracionColecciones.ColeccionTiposSensores);

        var resultado = await coleccionTiposSensores
            .ReplaceOneAsync(tipoSensor => tipoSensor.Id == unTipoSensor.Id, unTipoSensor);

        if (resultado.IsAcknowledged)
            resultadoAccion = true;

        return resultadoAccion;
    }
    public async Task<bool> RemoveAsync(string tipoSensorId)
    {
        if (!ObjectId.TryParse(tipoSensorId, out var tipoSensor_objectId))
            throw new FormatException($"El valor '{tipoSensorId}' no tiene un formato válido de ObjectId.");
        
        bool resultadoAccion = false;

        var conexion = _contextoDb
            .CreateConnection();

        var coleccionTiposSensores = conexion
            .GetCollection<TipoSensor>(_contextoDb.ConfiguracionColecciones.ColeccionTiposSensores);

        var resultado = await coleccionTiposSensores
            .DeleteOneAsync(tipoSensor => tipoSensor.Id == tipoSensorId);

        if (resultado.IsAcknowledged)
            resultadoAccion = true;

        return resultadoAccion;
    }
}