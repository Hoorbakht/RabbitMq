using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record MessagesReadyDetails(
    [property: JsonPropertyName("rate")] double? Rate
);