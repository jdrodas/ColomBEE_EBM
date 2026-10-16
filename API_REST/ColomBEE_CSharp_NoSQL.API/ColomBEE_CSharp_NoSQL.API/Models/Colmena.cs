using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ColomBEE_CSharp_NoSQL.API.Models;

public class Colmena
{
    [JsonPropertyName("id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = string.Empty;

    [JsonPropertyName("apiario_nombre")] 
    [BsonElement("apiario_nombre")]
    [BsonRepresentation(BsonType.String)]
    public string? ApiarioNombre { get; set; } = string.Empty;

    [JsonPropertyName("apiario_id")]
    [BsonElement("apiario_objectId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ApiarioId { get; set; } = string.Empty;

    [JsonPropertyName("nombre")] 
    [BsonElement("nombre")]
    [BsonRepresentation(BsonType.String)]
    public string? Nombre { get; set; } = string.Empty;

    [JsonPropertyName("fecha_instalacion")]
    [BsonElement("fecha_instalacion")]
    [BsonRepresentation(BsonType.String)]
    public string? FechaInstalacion { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var otraColmena = (Colmena)obj;

        return Id == otraColmena.Id
               && Nombre!.ToUpper().Equals(otraColmena.Nombre!.ToUpper())
               && ApiarioId == otraColmena.ApiarioId
               && FechaInstalacion! == otraColmena.FechaInstalacion;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 3;
            hash = hash * 5 + (Id?.GetHashCode() ?? 0);
            hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
            hash = hash * 5 + (ApiarioId?.GetHashCode() ?? 0);            
            hash = hash * 5 + (FechaInstalacion?.GetHashCode() ?? 0);

            return hash;
        }
    }
}