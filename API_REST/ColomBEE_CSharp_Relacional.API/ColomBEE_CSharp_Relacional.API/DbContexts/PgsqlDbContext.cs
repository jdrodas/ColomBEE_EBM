using Npgsql;
using System.Data;

namespace ColomBEE_CSharp_Relacional.API.DbContexts
{
    public class PgsqlDbContext(IConfiguration unaConfiguracion)
    {
        private readonly string _cadenaConexion = unaConfiguracion.GetConnectionString("ColomBEEPL")!;
        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_cadenaConexion);
        }
    }
}