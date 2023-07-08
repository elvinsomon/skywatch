using System.Text.Json.Serialization;

namespace MetricsShared.Common;

public class BaseResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorMessage { get; set; }
}