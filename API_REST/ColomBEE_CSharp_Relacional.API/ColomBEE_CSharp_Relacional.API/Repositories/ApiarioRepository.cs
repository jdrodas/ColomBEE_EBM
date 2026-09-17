using System.Data;
using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;
using Dapper;
using Npgsql;

namespace ColomBEE_CSharp_Relacional.API.Repositories;

public class ApiarioRepository(PgsqlDbContext unContexto) : IApiarioRepository
{
    private readonly PgsqlDbContext _contextoDb = unContexto;

    public async Task<List<Apiario>> GetAllAsync()
    {
        try
        {
            var conexion = _contextoDb.CreateConnection();

            var sentenciaSQL =
                "SELECT DISTINCT id, nombre, latitud, longitud " +
                "FROM core.apiarios " +
                "ORDER BY nombre ";

            var resultadoApiarios = await conexion
                .QueryAsync<Apiario>(sentenciaSQL, new DynamicParameters());

            return [.. resultadoApiarios];
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }
    }

    public async Task<Apiario> GetByIdAsync(Guid apiarioId)
    {
        Apiario unApiario = new();
        var conexion = _contextoDb.CreateConnection();

        DynamicParameters parametrosSentencia = new();
        parametrosSentencia.Add("@apiarioId", apiarioId,
            DbType.Guid, ParameterDirection.Input);

        var sentenciaSQL =
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

        var sentenciaSQL =
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

    public async Task<long> GetTotalAssociatedBeehivesAsync(Guid apiarioId)
    {
        var conexion = _contextoDb.CreateConnection();

        DynamicParameters parametrosSentencia = new();
        parametrosSentencia.Add("@apiarioId", apiarioId,
            DbType.Guid, ParameterDirection.Input);

        var sentenciaSql =
            "SELECT COUNT(id) total FROM core.colmenas " +
            "WHERE apiario_id = @apiarioId";

        var totalColmenas = await conexion
            .QueryFirstAsync<long>(sentenciaSql, parametrosSentencia);

        return totalColmenas;
    }
    
    public async Task<List<Colmena>> GetAssociatedBeehivesAsync(Guid apiarioId)
    {
        var conexion = _contextoDb.CreateConnection();

        DynamicParameters parametrosSentencia = new();
        parametrosSentencia.Add("@apiarioId", apiarioId,
            DbType.Guid, ParameterDirection.Input);

        var sentenciaSQL =
            "SELECT DISTINCT c.id, c.apiario_id ApiarioId, a.nombre ApiarioNombre, " +
            "c.codigo, to_char(c.fecha_instalacion,'DD/MM/YYYY') FechaInstalacion  " +
            "FROM core.colmenas c JOIN core.apiarios a ON " +
            "c.apiario_id = a.id " +
            "WHERE a.id = @apiarioId";

        var resultadoColmenas = await conexion
            .QueryAsync<Colmena>(sentenciaSQL, parametrosSentencia);
        

        return [.. resultadoColmenas];
    }

    public async Task<bool> CreateAsync(Apiario unApiario)
    {
        try
        {
            var resultadoAccion = false;
            var conexion = _contextoDb.CreateConnection();

            var procedimiento = "core.p_inserta_apiario";
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
            
            return resultadoAccion;            
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }
    }
    
    public async Task<bool> UpdateAsync(Apiario unApiario)
    {
        try
        {
            var resultadoAccion = false;
            var conexion = _contextoDb.CreateConnection();

            var procedimiento = "core.p_actualiza_apiario";
            var parametros = new
            {
                p_id = unApiario.Id,
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
            
            return resultadoAccion;            
        }
        catch (NpgsqlException error)
        {
            throw new DbOperationException(error.Message);
        }
    }

    public async Task<bool> RemoveAsync(Guid apiarioId)
    {
        try
        {
            var resultadoAccion = false;
            var conexion = _contextoDb.CreateConnection();

            var procedimiento = "core.p_elimina_apiario";
            var parametros = new
            {
                p_id = apiarioId
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