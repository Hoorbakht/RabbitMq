using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record PublishOutDetails(
    [property: JsonPropertyName("rate")] double? Rate
);