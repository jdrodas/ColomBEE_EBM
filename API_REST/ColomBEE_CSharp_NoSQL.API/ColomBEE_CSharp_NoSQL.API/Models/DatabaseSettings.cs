namespace ColomBEE_CSharp_NoSQL.API.Models;

public class DatabaseSettings
{
    public string Database { get; set; }
    public string ColeccionApiarios { get; set; }
    public string ColeccionColmenas { get; set; }
    public string ColeccionTiposSensores { get; set; }
    public string ColeccionSensores { get; set; }
    public string ColeccionLecturas { get; set; }
    
    public DatabaseSettings(IConfiguration unaConfiguracion)
    {
        var configuracion = unaConfiguracion.GetSection("DatabaseSettings");

        Database = configuracion.GetSection("Database").Value!;
        ColeccionApiarios = configuracion.GetSection("ApiariesCollection").Value!;
        ColeccionColmenas = configuracion.GetSection("BeehivesCollection").Value!;
        ColeccionTiposSensores = configuracion.GetSection("SensorsTypesCollection").Value!;
        ColeccionSensores = configuracion.GetSection("SensorsCollection").Value!;
        ColeccionLecturas = configuracion.GetSection("ReadingsCollection").Value!;
    }
}