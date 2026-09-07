using System.Text.Json.Serialization;

namespace ColomBEE_CSharp_Relacional.API.Models
{
    public class SaludSistema
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [JsonPropertyName("fecha_validacion")]
        public DateTime FechaValidacion { get; set; } = DateTime.UtcNow;
        
        [JsonPropertyName("estado")]
        public string Estado { get; set; } = string.Empty;
        
        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; } = string.Empty;
        
        [JsonPropertyName("detalle_error")]
        public string DetalleError { get; set; } = string.Empty;
        
        [JsonPropertyName("tiempo_respuesta_ms")]
        public long TiempoRespuestaMs { get; set; }
        
        [JsonPropertyName("database_conectada")] 
        public bool DatabaseConectada { get; set; } = false;
        
        [JsonPropertyName("tipo_validacion")]
        public string TipoValidacion { get; set; } = "SALUD";
        
        [JsonPropertyName("esta_saludable")]
        public bool EstaSaludable { get; set; } = false;   
    }
}