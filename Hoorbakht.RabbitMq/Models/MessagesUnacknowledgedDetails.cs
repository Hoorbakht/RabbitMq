using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record MessagesUnacknowledgedDetails(
    [property: JsonPropertyName("rate")] double? Rate
);