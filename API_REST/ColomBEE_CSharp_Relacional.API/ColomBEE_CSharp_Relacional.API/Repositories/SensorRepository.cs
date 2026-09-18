using System.Data;
using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;
using Dapper;
using Npgsql;

namespace ColomBEE_CSharp_Relacional.API.Repositories;

public class SensorRepository(PgsqlDbContext unContexto) : ISensorRepository
{
    private readonly PgsqlDbContext _contextoDb = unContexto;

    public async Task<List<Sensor>> GetAllAsync()
    {
        try
        {
            var conexion = _contextoDb.CreateConnection();

            var sentenciaSQL =
                "SELECT id, tipoId, colmenaId, " +
                "tipoNombre, colmenaCodigo, " +
                "frecuenciaMuestreo, " +
                "fechaInstalacion " +
                "FROM core.v_info_sensores " +
                "ORDER BY colmenaCodigo, tipoNombre";

            var resultadoSensores = await conexion
                .QueryAsync<Sensor>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoSensores];
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }
    }

    public async Task<Sensor> GetByIdAsync(Guid sensorId)
    {
        try
        {
            Sensor unSensor = new();
            var conexion = _contextoDb.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@sensorId", sensorId,
                DbType.Guid, ParameterDirection.Input);

            var sentenciaSQL =
                "SELECT id, tipoId, colmenaId, " +
                "tipoNombre, colmenaCodigo, " +
                "frecuenciaMuestreo, " +
                "fechaInstalacion " +
                "FROM core.v_info_sensores " +
                "WHERE id = @sensorId";

            var resultado = await conexion
                .QueryAsync<Sensor>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unSensor = resultado.First();

            return unSensor;
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }
    }

    public async Task<Sensor> GetByDetailsAsync(Sensor unSensor)
    {
        Sensor sensorEncontrado = new();
        var conexion = _contextoDb.CreateConnection();

        DynamicParameters parametrosSentencia = new();
        parametrosSentencia.Add("@tipoId", unSensor.TipoId,
            DbType.Guid, ParameterDirection.Input);
        parametrosSentencia.Add("@colmenaId", unSensor.ColmenaId,
            DbType.Guid, ParameterDirection.Input);
        parametrosSentencia.Add("@fechaInstalacion", unSensor.FechaInstalacion!,
            DbType.String, ParameterDirection.Input);

        var sentenciaSQL =
            "SELECT id, tipoId, colmenaId, " +
            "tipoNombre, colmenaCodigo, " +
            "frecuenciaMuestreo, " +
            "fechaInstalacion " +
            "FROM core.v_info_sensores " +
            "WHERE tipoId = @tipoId " +
            "AND colmenaId = @colmenaId " +
            "AND fechaInstalacion = @fechaInstalacion";

        var resultado = await conexion
            .QueryAsync<Sensor>(sentenciaSQL, parametrosSentencia);

        if (resultado.Any())
            sensorEncontrado = resultado.First();

        return sensorEncontrado;
    }

    public async Task<long> GetTotalAssociatedReadingsAsync(Guid sensorId)
    {
        var conexion = _contextoDb.CreateConnection();

        DynamicParameters parametrosSentencia = new();
        parametrosSentencia.Add("@sensorId", sensorId,
            DbType.Guid, ParameterDirection.Input);

        var sentenciaSql =
            "SELECT COUNT(id) total FROM core.lecturas " +
            "WHERE sensor_id = @sensorId";

        var totalLecturas = await conexion
            .QueryFirstAsync<long>(sentenciaSql, parametrosSentencia);

        return totalLecturas;
    }
    
    public async Task<List<Lectura>> GetAssociatedReadingsAsync(Guid sensorId)
    {
        var conexion = _contextoDb.CreateConnection();

        DynamicParameters parametrosSentencia = new();
        parametrosSentencia.Add("@sensorId", sensorId,
            DbType.Guid, ParameterDirection.Input);

        var sentenciaSQL =
            "SELECT DISTINCT id, sensor_id sensorId, fecha_registro fechaRegistro, valor " +
            "FROM core.lecturas " +
            "WHERE sensor_id = @sensorId";
            
        var resultadoLecturas = await conexion
            .QueryAsync<Lectura>(sentenciaSQL, parametrosSentencia);
        
        return [.. resultadoLecturas];
    }

    public async Task<bool> CreateAsync(Sensor unSensor)
    {
        try
        {
            var resultadoAccion = false;
            var conexion = _contextoDb.CreateConnection();

            var procedimiento = "core.p_inserta_sensor";
            var parametros = new
            {
                p_colmena_id = unSensor.ColmenaId,
                p_tipo_id = unSensor.TipoId,
                p_frecuencia_muestreo = unSensor.FrecuenciaMuestreo,
                p_fecha_instalacion = unSensor.FechaInstalacion
            };

            var cantidad_filas = await conexion.ExecuteAsync(
                procedimiento,
                parametros,
                commandType: CommandType.StoredProcedure);

            if (cantidad_filas != 0)
                resultadoAccion = true;

            return resultadoAccion;
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }
    }
    
    public async Task<bool> UpdateAsync(Sensor unSensor)
    {
        try
        {
            var resultadoAccion = false;
            var conexion = _contextoDb.CreateConnection();

            var procedimiento = "core.p_actualiza_sensor";
            var parametros = new
            {
                p_id = unSensor.Id,
                p_colmena_id = unSensor.ColmenaId,
                p_tipo_id = unSensor.TipoId,
                p_frecuencia_muestreo = unSensor.FrecuenciaMuestreo,
                p_fecha_instalacion = unSensor.FechaInstalacion
            };

            var cantidad_filas = await conexion.ExecuteAsync(
                procedimiento,
                parametros,
                commandType: CommandType.StoredProcedure);

            if (cantidad_filas != 0)
                resultadoAccion = true;
            
            return resultadoAccion;            
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }
    }

    public async Task<bool> RemoveAsync(Guid sensorId)
    {
        try
        {
            var resultadoAccion = false;
            var conexion = _contextoDb.CreateConnection();

            var procedimiento = "core.p_elimina_sensor";
            var parametros = new
            {
                p_id = sensorId
            };

            var cantidad_filas = await conexion.ExecuteAsync(
                procedimiento,
                parametros,
                commandType: CommandType.StoredProcedure);

            if (cantidad_filas != 0)
                resultadoAccion = true;

            return resultadoAccion;
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }
    }
}