using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record MessagesDetails(
    [property: JsonPropertyName("rate")] double? Rate
);