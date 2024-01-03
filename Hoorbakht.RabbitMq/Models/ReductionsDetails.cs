using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record ReductionsDetails(
    [property: JsonPropertyName("rate")] double? Rate
);