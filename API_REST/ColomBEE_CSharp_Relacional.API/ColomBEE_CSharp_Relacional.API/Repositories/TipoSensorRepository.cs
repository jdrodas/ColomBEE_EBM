using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using Dapper;
using System.Data;

namespace ColomBEE_CSharp_Relacional.API.Repositories
{
    public class TipoSensorRepository(PgsqlDbContext unContexto) : ITipoSensorRepository
    {
        private readonly PgsqlDbContext _contextoDb = unContexto;
        
        public async Task<List<TipoSensor>> GetAllAsync()
        {
            var conexion = _contextoDb.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, unidad_medida UnidadMedida " +
                "FROM core.tipos_sensores " +
                "ORDER BY nombre ";

            var resultadoTiposSensores = await conexion
                .QueryAsync<TipoSensor>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoTiposSensores];
        }
        
        public async Task<TipoSensor> GetByIdAsync(Guid tipoSensorId)
        {
            TipoSensor unTipoSensor = new();
            var conexion = _contextoDb.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@tipoSensorId", tipoSensorId,
                DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, unidad_medida UnidadMedida " +
                "FROM core.tipos_sensores " +
                "WHERE id = @tipoSensorId ";

            var resultado = await conexion
                .QueryAsync<TipoSensor>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unTipoSensor = resultado.First();

            return unTipoSensor;
        }
    }
}