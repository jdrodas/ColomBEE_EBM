using System.Text.Json.Serialization;

namespace ColomBEE_CSharp_Relacional.API.Models
{
    public class Sensor
    {
        [JsonPropertyName("id")] 
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("tipo_id")] 
        public Guid TipoId { get; set; } = Guid.Empty;

        [JsonPropertyName("colmena_id")] 
        public Guid ColmenaId { get; set; } = Guid.Empty;

        [JsonPropertyName("tipo_nombre")] 
        public string? TipoNombre { get; set; } = string.Empty;

        [JsonPropertyName("colmena_codigo")] 
        public string? ColmenaCodigo { get; set; } = string.Empty;

        [JsonPropertyName("frecuencia_muestreo")]
        public int FrecuenciaMuestreo { get; set; } = 0;

        [JsonPropertyName("fecha_instalacion")]
        public string? FechaInstalacion { get; set; } = string.Empty;

    public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var otroSensor = (Sensor)obj;

            return Id == otroSensor.Id
                   && TipoId == otroSensor.TipoId
                   && ColmenaId == otroSensor.ColmenaId
                   && FrecuenciaMuestreo == otroSensor.FrecuenciaMuestreo
                   && FechaInstalacion == otroSensor.FechaInstalacion;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 3;
                hash = hash * 5 + Id.GetHashCode();
                hash = hash * 5 + TipoId.GetHashCode();
                hash = hash * 5 + ColmenaId.GetHashCode();
                hash = hash * 5 + (FechaInstalacion?.GetHashCode() ?? 0);
                hash = hash * 5 + FrecuenciaMuestreo.GetHashCode();

                return hash;
            }
        }
    }
}