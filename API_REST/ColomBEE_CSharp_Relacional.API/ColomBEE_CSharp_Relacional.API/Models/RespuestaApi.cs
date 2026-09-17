using System.Text.Json.Serialization;

namespace ColomBEE_CSharp_Relacional.API.Models;

public class RespuestaApi
{
    [JsonPropertyName("status_code")] public int StatusCode { get; set; }

    [JsonPropertyName("mensaje")] public string Mensaje { get; set; } = string.Empty;
}