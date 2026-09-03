using System.Text.Json.Serialization;

namespace ColomBEE_CSharp_Relacional.API.Models
{
    public class Estadistica
    {
        [JsonPropertyName("lecturas")]
        public long Lecturas { get; set; } = 0;

        [JsonPropertyName("sensores")]
        public long Sensores { get; set; } = 0;

        [JsonPropertyName("apiarios")]
        public long Apiarios { get; set; } = 0;

        [JsonPropertyName("colmenas")]
        public long Colmenas { get; set; } = 0;
        
        [JsonPropertyName("tipos_sensores")]
        public long Tipos_Sensores { get; set; } = 0;
    }
}