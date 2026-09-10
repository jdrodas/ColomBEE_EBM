using System.Text.Json.Serialization;

namespace ColomBEE_CSharp_Relacional.API.Models
{
    public class Apiario
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;
        
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;
        
        [JsonPropertyName("latitud")]
        public double? Latitud { get; set; } = 0.0d;
        
        [JsonPropertyName("longitud")]
        public double? Longitud { get; set; } = 0.0d;
        
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroApiario = (Apiario)obj;

            return Id == otroApiario.Id
                   && Nombre!.Equals(otroApiario.Nombre)
                   && Latitud == otroApiario.Latitud
                   && Longitud == otroApiario.Longitud;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + Latitud.GetHashCode();
                hash = hash * 5 + Longitud.GetHashCode();

                return hash;
            }
        }
    }
}