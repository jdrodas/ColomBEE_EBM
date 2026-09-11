using System.Text.Json.Serialization;

namespace ColomBEE_CSharp_Relacional.API.Models
{
    public class Colmena
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;
        
        [JsonPropertyName("apiario_nombre")]
        public string? ApiarioNombre { get; set; } = string.Empty;
        
        [JsonPropertyName("apiario_id")]
        public Guid ApiarioId { get; set; } = Guid.Empty;
        
        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; } = string.Empty;

        [JsonPropertyName("fecha_instalacion")]
        public string? FechaInstalacion { get; set; } = string.Empty;
        
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otraColmena = (Colmena)obj;

            return Id == otraColmena.Id
                   && Codigo!.ToUpper().Equals(otraColmena.Codigo!.ToUpper())
                   && ApiarioId == otraColmena.ApiarioId
                   && FechaInstalacion! == otraColmena.FechaInstalacion;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Codigo?.GetHashCode() ?? 0);
                hash = hash * 5 + ApiarioId.GetHashCode();
                hash = hash * 5 + (FechaInstalacion?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}