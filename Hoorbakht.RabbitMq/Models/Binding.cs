using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record Binding(
    [property: JsonPropertyName("source")] string Source,
    [property: JsonPropertyName("vhost")] string Vhost,
    [property: JsonPropertyName("destination")] string Destination,
    [property: JsonPropertyName("destination_type")] string DestinationType,
    [property: JsonPropertyName("routing_key")] string RoutingKey,
    [property: JsonPropertyName("arguments")] Arguments Arguments,
    [property: JsonPropertyName("properties_key")] string PropertiesKey
);