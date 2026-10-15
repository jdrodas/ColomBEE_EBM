using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ColomBEE_CSharp_NoSQL.API.Models;

public class TipoSensor
{
    [JsonPropertyName("id")]
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = string.Empty;

    [JsonPropertyName("nombre")]
    [BsonElement("nombre")]
    [BsonRepresentation(BsonType.String)]
    public string? Nombre { get; set; } = string.Empty;

    [JsonPropertyName("unidad_medida")] 
    [BsonElement("unidad_medida")]
    [BsonRepresentation(BsonType.String)]
    public string? UnidadMedida { get; set; } = string.Empty;


    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var otroTipoSensor = (TipoSensor)obj;

        return Id == otroTipoSensor.Id
               && Nombre!.ToUpper().Equals(otroTipoSensor.Nombre!.ToUpper())
               && UnidadMedida!.ToUpper()! == otroTipoSensor.UnidadMedida!.ToUpper();
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 3;
            hash = hash * 5 + (Id?.GetHashCode() ?? 0);
            hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
            hash = hash * 5 + (UnidadMedida?.GetHashCode() ?? 0);

            return hash;
        }
    }
}