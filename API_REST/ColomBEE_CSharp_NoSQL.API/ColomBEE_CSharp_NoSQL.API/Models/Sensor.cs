using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ColomBEE_CSharp_NoSQL.API.Models;

public class Sensor
{
    [JsonPropertyName("id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = string.Empty;
    
    [JsonPropertyName("colmena_id")] 
    [BsonElement("colmena_objectId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ColmenaId { get; set; } = string.Empty;

    [JsonPropertyName("tipo_id")]
    [BsonElement("tipo_sensor_objectId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? TipoId { get; set; } = string.Empty;
    
    [JsonPropertyName("frecuencia_muestreo")]
    [BsonElement("frecuencia_muestreo")]
    [BsonRepresentation(BsonType.Int32)]
    public int FrecuenciaMuestreo { get; set; } = 0;

    [JsonPropertyName("fecha_instalacion")]
    [BsonElement("fecha_instalacion")]
    [BsonRepresentation(BsonType.String)]
    public string? FechaInstalacion { get; set; } = string.Empty;

    [JsonPropertyName("tipo_sensor_nombre")] 
    [BsonElement("tipo_sensor_nombre")]
    [BsonRepresentation(BsonType.String)]
    public string? TipoSensorNombre { get; set; } = string.Empty;

    [JsonPropertyName("colmena_nombre")]
    [BsonElement("colmena_nombre")]
    [BsonRepresentation(BsonType.String)]
    public string? ColmenaNombre { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var otroSensor = (Sensor)obj;

        return TipoId == otroSensor.TipoId
               && ColmenaId == otroSensor.ColmenaId
               && FrecuenciaMuestreo == otroSensor.FrecuenciaMuestreo
               && FechaInstalacion == otroSensor.FechaInstalacion;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 3;
            hash = hash * 5 + (Id?.GetHashCode() ?? 0);
            hash = hash * 5 + (TipoId?.GetHashCode() ?? 0);
            hash = hash * 5 + (ColmenaId?.GetHashCode() ?? 0);
            hash = hash * 5 + (FechaInstalacion?.GetHashCode() ?? 0);
            hash = hash * 5 + FrecuenciaMuestreo.GetHashCode();

            return hash;
        }
    }
}