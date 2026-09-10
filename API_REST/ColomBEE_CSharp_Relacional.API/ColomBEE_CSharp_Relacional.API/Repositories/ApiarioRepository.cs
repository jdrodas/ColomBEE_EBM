using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using Dapper;
using System.Data;

namespace ColomBEE_CSharp_Relacional.API.Repositories
{
    public class ApiarioRepository(PgsqlDbContext unContexto) : IApiarioRepository
    {
        private readonly PgsqlDbContext _contextoDb = unContexto;
        
        public async Task<List<Apiario>> GetAllAsync()
        {
            var conexion = _contextoDb.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, latitud, longitud " +
                "FROM core.apiarios " +
                "ORDER BY nombre ";

            var resultadoApiarios = await conexion
                .QueryAsync<Apiario>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoApiarios];
        }
        
        public async Task<Apiario> GetByIdAsync(Guid apiarioId)
        {
            Apiario unApiario = new();
            var conexion = _contextoDb.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@apiarioId", apiarioId,
                DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, latitud, longitud " +
                "FROM core.apiarios " +
                "WHERE id = @apiarioId";

            var resultado = await conexion
                .QueryAsync<Apiario>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unApiario = resultado.First();

            return unApiario;
        }
    }
}