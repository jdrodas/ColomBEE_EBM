using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using Npgsql;
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
        
        public async Task<Colmena> GetByDetailsAsync(Colmena unaColmena)
        {
            Colmena colmenaEncontrada = new();
            var conexion = _contextoDb.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@apiarioId", unaColmena.ApiarioId,
                DbType.Guid, ParameterDirection.Input);
            parametrosSentencia.Add("@colmenaCodigo", unaColmena.Codigo,
                DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@fechaInstalacion", unaColmena.FechaInstalacion,
                DbType.String, ParameterDirection.Input);
            
            string sentenciaSQL =
                "SELECT DISTINCT c.id, c.apiario_id ApiarioId, a.nombre ApiarioNombre, " +
                "c.codigo, to_char(c.fecha_instalacion,'DD/MM/YYYY') FechaInstalacion  " +
                "FROM core.colmenas c JOIN core.apiarios a ON c.apiario_id = a.id " +
                "WHERE c.apiario_id = @apiarioId " +
                "AND upper(c.codigo) = upper(@colmenaCodigo) " +
                "AND c.fecha_instalacion  = to_date(@fechaInstalacion,'DD/MM/YYYY')";

            var resultado = await conexion
                .QueryAsync<Colmena>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                colmenaEncontrada = resultado.First();

            return colmenaEncontrada;
        }     
        
        public async Task<bool> CreateAsync(Colmena unaColmena)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = _contextoDb.CreateConnection();

                string procedimiento = "core.p_inserta_colmena";
                var parametros = new
                {
                    p_codigo = unaColmena.Codigo,
                    p_apiario_id = unaColmena.ApiarioId,
                    p_fecha_instalacion = unaColmena.FechaInstalacion
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