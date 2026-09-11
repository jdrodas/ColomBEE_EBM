using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Models;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using Dapper;
using Npgsql;
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
        public async Task<Apiario> GetByDetailsAsync(Apiario unApiario)
        {
            Apiario apiarioEncontrado = new();
            var conexion = _contextoDb.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@apiarioNombre", unApiario.Nombre,
                DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@apiarioLatitud", unApiario.Latitud,
                DbType.Double, ParameterDirection.Input);
            parametrosSentencia.Add("@apiarioLongitud", unApiario.Longitud,
                DbType.Double, ParameterDirection.Input);
            
            string sentenciaSQL =
                "SELECT DISTINCT id, nombre, latitud, longitud " +
                "FROM core.apiarios " +
                "WHERE upper(nombre) = upper(@apiarioNombre) " +
                "AND latitud = @apiarioLatitud " +
                "AND longitud = @apiarioLongitud";

            var resultado = await conexion
                .QueryAsync<Apiario>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                apiarioEncontrado = resultado.First();

            return apiarioEncontrado;
        }        
        
        public async Task<bool> CreateAsync(Apiario unApiario)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = _contextoDb.CreateConnection();

                string procedimiento = "core.p_inserta_apiario";
                var parametros = new
                {
                    p_nombre = unApiario.Nombre,
                    p_latitud = unApiario.Latitud,
                    p_longitud = unApiario.Longitud
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