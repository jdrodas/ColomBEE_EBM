using System.Data;
using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;
using Dapper;
using Npgsql;

namespace ColomBEE_CSharp_Relacional.API.Repositories;

public class TipoSensorRepository(PgsqlDbContext unContexto) : ITipoSensorRepository
{
    private readonly PgsqlDbContext _contextoDb = unContexto;

    public async Task<List<TipoSensor>> GetAllAsync()
    {
        try
        {
            var conexion = _contextoDb.CreateConnection();

            var sentenciaSQL =
                "SELECT DISTINCT id, nombre, unidad_medida UnidadMedida " +
                "FROM core.tipos_sensores " +
                "ORDER BY nombre ";

            var resultadoTiposSensores = await conexion
                .QueryAsync<TipoSensor>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoTiposSensores];
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }
    }

    public async Task<TipoSensor> GetByIdAsync(Guid tipoSensorId)
    {
        try
        {
            TipoSensor unTipoSensor = new();
            var conexion = _contextoDb.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@tipoSensorId", tipoSensorId,
                DbType.Guid, ParameterDirection.Input);

            var sentenciaSQL =
                "SELECT DISTINCT id, nombre, unidad_medida UnidadMedida " +
                "FROM core.tipos_sensores " +
                "WHERE id = @tipoSensorId ";

            var resultado = await conexion
                .QueryAsync<TipoSensor>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unTipoSensor = resultado.First();

            return unTipoSensor;
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }
    }

    public async Task<TipoSensor> GetByDetailsAsync(TipoSensor unTipoSensor)
    {
        TipoSensor tipoSensorEncontrado = new();
        var conexion = _contextoDb.CreateConnection();

        DynamicParameters parametrosSentencia = new();
        parametrosSentencia.Add("@tipoSensorNombre", unTipoSensor.Nombre,
            DbType.String, ParameterDirection.Input);
        parametrosSentencia.Add("@unidadMedida", unTipoSensor.UnidadMedida,
            DbType.String, ParameterDirection.Input);

        var sentenciaSQL =
            "SELECT DISTINCT id, nombre, unidad_medida UnidadMedida " +
            "FROM core.tipos_sensores " +
            "WHERE upper(nombre) = upper(@tipoSensorNombre) " +
            "AND upper(unidad_medida) = upper(@unidadMedida)";

        var resultado = await conexion
            .QueryAsync<TipoSensor>(sentenciaSQL, parametrosSentencia);

        if (resultado.Any())
            tipoSensorEncontrado = resultado.First();

        return tipoSensorEncontrado;
    }

    public async Task<long> GetTotalAssociatedSensorsAsync(Guid tipoSensorId)
    {
        var conexion = _contextoDb.CreateConnection();

        DynamicParameters parametrosSentencia = new();
        parametrosSentencia.Add("@tipoSensorId", tipoSensorId,
            DbType.Guid, ParameterDirection.Input);

        var sentenciaSql =
            "SELECT COUNT(id) total FROM core.sensores " +
            "WHERE tipo_id = @tipoSensorId";

        var totalSensores = await conexion
            .QueryFirstAsync<long>(sentenciaSql, parametrosSentencia);

        return totalSensores;
    }

    public async Task<bool> CreateAsync(TipoSensor unTipoSensor)
    {
        try
        {
            var resultadoAccion = false;
            var conexion = _contextoDb.CreateConnection();

            var procedimiento = "core.p_inserta_tipo_sensor";
            var parametros = new
            {
                p_nombre = unTipoSensor.Nombre,
                p_unidad_medida = unTipoSensor.UnidadMedida
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

    public async Task<bool> RemoveAsync(Guid tipoSensorId)
    {
        try
        {
            var resultadoAccion = false;
            var conexion = _contextoDb.CreateConnection();

            var procedimiento = "core.p_elimina_tipo_sensor";
            var parametros = new
            {
                p_id = tipoSensorId
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