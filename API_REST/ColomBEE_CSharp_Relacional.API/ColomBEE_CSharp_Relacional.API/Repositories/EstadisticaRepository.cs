using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;
using Dapper;

namespace ColomBEE_CSharp_Relacional.API.Repositories
{
    public class EstadisticaRepository(PgsqlDbContext unContexto) : IEstadisticaRepository
    {
        private readonly PgsqlDbContext _contextoDb = unContexto;

        public async Task<Estadistica> GetAllAsync()
        {
            var conexion = _contextoDb.CreateConnection();

            Estadistica conteoRegistros = new();

            var sentenciaSql =
                "SELECT COUNT(id) total FROM core.apiarios";

            conteoRegistros.Apiarios = await conexion
                .QueryFirstAsync<long>(sentenciaSql, new DynamicParameters());

            sentenciaSql =
                "SELECT COUNT(id) total FROM core.colmenas";

            conteoRegistros.Colmenas = await conexion
                .QueryFirstAsync<long>(sentenciaSql, new DynamicParameters());

            sentenciaSql =
                "SELECT COUNT(id) total FROM core.sensores";

            conteoRegistros.Sensores = await conexion
                .QueryFirstAsync<long>(sentenciaSql, new DynamicParameters());

            sentenciaSql =
                "SELECT COUNT(id) total FROM core.tipos_sensores";

            conteoRegistros.Tipos_Sensores = await conexion
                .QueryFirstAsync<long>(sentenciaSql, new DynamicParameters());

            sentenciaSql =
                "SELECT COUNT(id) total FROM core.lecturas";

            conteoRegistros.Lecturas = await conexion
                .QueryFirstAsync<long>(sentenciaSql, new DynamicParameters());

            return conteoRegistros;
        }
    }
}