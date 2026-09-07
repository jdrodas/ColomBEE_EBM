using ColomBEE_CSharp_Relacional.API.DbContexts;
using ColomBEE_CSharp_Relacional.API.Exceptions;
using ColomBEE_CSharp_Relacional.API.Interfaces;
using ColomBEE_CSharp_Relacional.API.Models;
using System.Diagnostics;
using Dapper;
using Npgsql;

namespace ColomBEE_CSharp_Relacional.API.Repositories
{
    public class HealthCheckRepository(PgsqlDbContext unContexto) : IHealthCheckRepository
    {
        private readonly PgsqlDbContext _contextoDb = unContexto;

        public async Task<SaludSistema> GetHealthAsync()
        {
            var cronometro = Stopwatch.StartNew();
            var resultado = new SaludSistema();
            
            try
            {
                using var conexion = _contextoDb.CreateConnection();                

                var resultadoConsulta = await conexion.ExecuteScalarAsync<int?>("SELECT 1");
                cronometro.Stop();

                resultado.DatabaseConectada = resultadoConsulta == 1;
                resultado.TiempoRespuestaMs = cronometro.ElapsedMilliseconds;
                resultado.Estado = "OPERATIVO";
                resultado.EstaSaludable = true;
                resultado.Mensaje = resultado.DatabaseConectada
                    ? "Base de datos disponible"
                    : "La base de datos no respondió correctamente";
            }
            catch (Exception unError) when (unError is DbOperationException or NpgsqlException)
            {
                cronometro.Stop();
                resultado.DatabaseConectada = false;
                resultado.TiempoRespuestaMs = cronometro.ElapsedMilliseconds;
                resultado.Estado = "FALLIDO";
                resultado.Mensaje = "Error al conectar con la base de datos";
                resultado.DetalleError = $"{unError.GetType().Name}: {unError.Message}";
            }

            return resultado;
        }
    }
}