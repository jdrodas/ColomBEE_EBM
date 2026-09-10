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
                "SELECT s.id, s.tipo_id tipoId, s.colmena_id colmenaId, "+
                "ts.nombre tipoNombre, c.codigo colmenaCodigo, " +
                "s.frecuencia_muestreo frecuenciaMuestreo, " +
                "to_char(s.fecha_instalacion,'DD/MM/YYYY') fechaInstalacion "+
                "FROM core.sensores s " +
                "JOIN core.tipos_sensores ts ON s.tipo_id = ts.id "+
                "JOIN core.colmenas c ON s.colmena_id = c.id "+
                "ORDER BY c.codigo, ts.nombre";

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
                "SELECT s.id, s.tipo_id tipoId, s.colmena_id colmenaId, "+
                "ts.nombre tipoNombre, c.codigo colmenaCodigo, " +
                "s.frecuencia_muestreo frecuenciaMuestreo, " +
                "to_char(s.fecha_instalacion,'DD/MM/YYYY') fechaInstalacion "+
                "FROM core.sensores s " +
                "JOIN core.tipos_sensores ts ON s.tipo_id = ts.id "+
                "JOIN core.colmenas c ON s.colmena_id = c.id "+
                "WHERE s.id = @sensorId";

            var resultado = await conexion
                .QueryAsync<Sensor>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unSensor = resultado.First();

            return unSensor;
        }
    }
}