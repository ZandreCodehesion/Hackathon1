using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Patric;

public class VerificationResponse
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
