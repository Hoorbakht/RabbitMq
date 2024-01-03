using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record PublishInDetails(
    [property: JsonPropertyName("rate")] double? Rate
);