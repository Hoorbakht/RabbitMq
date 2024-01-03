using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record MessageStats(
    [property: JsonPropertyName("publish_in")] int? PublishIn,
    [property: JsonPropertyName("publish_in_details")] PublishInDetails PublishInDetails,
    [property: JsonPropertyName("publish_out")] int? PublishOut,
    [property: JsonPropertyName("publish_out_details")] PublishOutDetails PublishOutDetails
);