using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using Dapper;
using System.Data;

namespace ColomBEE_CSharp_Relacional.API.Repositories
{
    public class ColmenaRepository(PgsqlDbContext unContexto) : IColmenaRepository
    {
        private readonly PgsqlDbContext _contextoDb = unContexto;
        
        public async Task<List<Colmena>> GetAllAsync()
        {
            var conexion = _contextoDb.CreateConnection();

            string sentenciaSQL =
                "SELECT DISTINCT c.id, c.apiario_id ApiarioId, a.nombre ApiarioNombre, " +
                "c.codigo, to_char(c.fecha_instalacion,'DD/MM/YYYY') FechaInstalacion  " +
                "FROM core.colmenas c JOIN core.apiarios a ON " +
                "c.apiario_id = a.id " +
                "ORDER BY a.nombre ";

            var resultadoColmenas = await conexion
                .QueryAsync<Colmena>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoColmenas];
        }
        
        public async Task<Colmena> GetByIdAsync(Guid colmenaId)
        {
            Colmena unaColmena = new();
            var conexion = _contextoDb.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@colmenaId", colmenaId,
                DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL =
                "SELECT DISTINCT c.id, c.apiario_id ApiarioId, a.nombre ApiarioNombre, " +
                "c.codigo, to_char(c.fecha_instalacion,'DD/MM/YYYY') FechaInstalacion  " +
                "FROM core.colmenas c JOIN core.apiarios a ON " +
                "c.apiario_id = a.id " +
                "WHERE c.id = @colmenaId";

            var resultado = await conexion
                .QueryAsync<Colmena>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaColmena = resultado.First();

            return unaColmena;
        }
    }
}