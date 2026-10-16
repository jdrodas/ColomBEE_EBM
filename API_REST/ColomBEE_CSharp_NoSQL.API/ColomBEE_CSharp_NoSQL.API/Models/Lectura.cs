using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ColomBEE_CSharp_NoSQL.API.Models;

public class Lectura
{
    [JsonPropertyName("id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = string.Empty;
    
    [JsonPropertyName("sensor_id")]
    [BsonElement("sensor_objectId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? SensorId { get; set; } = string.Empty;
    
    [JsonPropertyName("fecha_registro")] 
    [BsonElement("fecha_registro")]
    [BsonRepresentation(BsonType.String)]
    public string? FechaRegistro { get; set; } = string.Empty;
 
    [JsonPropertyName("valor")]
    [BsonElement("valor")]
    [BsonRepresentation(BsonType.Double)]
    public double Valor { get; set; } = 0.0d;
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var otraLectura = (Lectura)obj;

        return SensorId == otraLectura.SensorId
               && FechaRegistro == otraLectura.FechaRegistro
               && Valor == otraLectura.Valor;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 3;
            hash = hash * 5 + (Id?.GetHashCode() ?? 0);
            hash = hash * 5 + (SensorId?.GetHashCode() ?? 0);
            hash = hash * 5 + (FechaRegistro?.GetHashCode() ?? 0);
            hash = hash * 5 + Valor.GetHashCode();

            return hash;
        }
    }
}