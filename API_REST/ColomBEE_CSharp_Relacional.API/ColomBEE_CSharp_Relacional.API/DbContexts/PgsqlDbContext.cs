using System.Data;
using Npgsql;

namespace ColomBEE_CSharp_Relacional.API.DbContexts;

public class PgsqlDbContext(IConfiguration unaConfiguracion)
{
    private readonly string _cadenaConexion = unaConfiguracion.GetConnectionString("ColomBEEPL")!;

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_cadenaConexion);
    }
}