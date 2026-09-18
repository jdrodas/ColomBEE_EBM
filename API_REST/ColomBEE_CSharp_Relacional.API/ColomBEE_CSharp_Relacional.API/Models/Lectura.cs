using System.Text.Json.Serialization;

namespace ColomBEE_CSharp_Relacional.API.Models;

public class Lectura
{
    [JsonPropertyName("id")] public Guid Id { get; set; } = Guid.Empty;
    [JsonPropertyName("sensor_id")] public Guid SensorId { get; set; } = Guid.Empty;
    [JsonPropertyName("fecha_registro")] public string? FechaRegistro { get; set; } = string.Empty;
    [JsonPropertyName("valor")] public double Valor { get; set; } = 0.0d;
    
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
            hash = hash * 5 + Id.GetHashCode();
            hash = hash * 5 + SensorId.GetHashCode();
            hash = hash * 5 + (FechaRegistro?.GetHashCode() ?? 0);
            hash = hash * 5 + Valor.GetHashCode();

            return hash;
        }
    }
}