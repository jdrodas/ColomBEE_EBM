using System.Text.Json.Serialization;

namespace ColomBEE_CSharp_Relacional.API.Models
{
    public class TipoSensor
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;
        
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;
        
        [JsonPropertyName("unidad_medida")]
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
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + (Nombre?.GetHashCode() ?? 0);
                hash = hash * 5 + (UnidadMedida?.GetHashCode() ?? 0);

                return hash;
            }
        }
    }
}