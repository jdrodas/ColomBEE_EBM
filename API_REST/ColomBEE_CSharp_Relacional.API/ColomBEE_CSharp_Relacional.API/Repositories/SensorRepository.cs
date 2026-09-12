using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using Dapper;
using System.Data;
using Npgsql;

namespace ColomBEE_CSharp_Relacional.API.Repositories
{
    public class SensorRepository(PgsqlDbContext unContexto) : ISensorRepository
    {
        private readonly PgsqlDbContext _contextoDb = unContexto;
        
        public async Task<List<Sensor>> GetAllAsync()
        {
            var conexion = _contextoDb.CreateConnection();

            string sentenciaSQL =
                "SELECT id, tipoId, colmenaId, "+
                "tipoNombre, colmenaCodigo, " +
                "frecuenciaMuestreo, " +
                "fechaInstalacion "+
                "FROM core.v_info_sensores " +
                "ORDER BY colmenaCodigo, tipoNombre";

            var resultadoSensores = await conexion
                .QueryAsync<Sensor>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoSensores];
        }
        
        public async Task<Sensor> GetByIdAsync(Guid sensorId)
        {
            Sensor unSensor = new();
            var conexion = _contextoDb.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@sensorId", sensorId,
                DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT id, tipoId, colmenaId, "+
                "tipoNombre, colmenaCodigo, " +
                "frecuenciaMuestreo, " +
                "fechaInstalacion "+
                "FROM core.v_info_sensores " +
                "WHERE id = @sensorId";

            var resultado = await conexion
                .QueryAsync<Sensor>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unSensor = resultado.First();

            return unSensor;
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
            parametrosSentencia.Add("@frecuenciaMuestreo", unSensor.FrecuenciaMuestreo,
                DbType.Int32, ParameterDirection.Input);
            
            string sentenciaSQL =
                "SELECT id, tipoId, colmenaId, "+
                "tipoNombre, colmenaCodigo, " +
                "frecuenciaMuestreo, " +
                "fechaInstalacion "+
                "FROM core.v_info_sensores " +
                "WHERE tipoId = @tipoId " +
                "AND colmenaId = @colmenaId " +
                "AND frecuenciaMuestreo = @frecuenciaMuestreo " +
                "AND fechaInstalacion = @fechaInstalacion";

            var resultado = await conexion
                .QueryAsync<Sensor>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                sensorEncontrado = resultado.First();

            return sensorEncontrado;
        }    
        
        public async Task<bool> CreateAsync(Sensor unSensor)
                {
                    bool resultadoAccion = false;
        
                    try
                    {
                        var conexion = _contextoDb.CreateConnection();
        
                        string procedimiento = "core.p_inserta_sensor";
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
                    }
                    catch (NpgsqlException error)
                    {
                        throw new DbOperationException(error.Message);
                    }
        
                    return resultadoAccion;
                }
    }
}