using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using Dapper;
using System.Data;

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
    }
}