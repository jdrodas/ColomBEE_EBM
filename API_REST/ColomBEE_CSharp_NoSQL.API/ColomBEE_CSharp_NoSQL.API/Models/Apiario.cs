using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ColomBEE_CSharp_NoSQL.API.Models;

public class Apiario
{
    [JsonPropertyName("id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = string.Empty;

    [JsonPropertyName("nombre")] 
    [BsonElement("nombre")]
    [BsonRepresentation(BsonType.String)]
    public string? Nombre { get; set; } = string.Empty;

    [JsonPropertyName("latitud")] 
    [BsonElement("latitud")]
    [BsonRepresentation(BsonType.Double)]
    public double? Latitud { get; set; } = 0.0d;

    [JsonPropertyName("longitud")] 
    [BsonElement("longitud")]
    [BsonRepresentation(BsonType.Double)]
    public double? Longitud { get; set; } = 0.0d;

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var otroApiario = (Apiario)obj;

        return Id == otroApiario.Id
               && Nombre!.ToUpper().Equals(otroApiario.Nombre!.ToUpper())
               && Latitud == otroApiario.Latitud
               && Longitud == otroApiario.Longitud;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 3;
            hash = hash * 5 + (Id?.GetHashCode() ?? 0);
            hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
            hash = hash * 5 + Latitud.GetHashCode();
            hash = hash * 5 + Longitud.GetHashCode();

            return hash;
        }
    }
}