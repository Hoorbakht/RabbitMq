using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record Exchange(
    [property: JsonPropertyName("arguments")] Arguments Arguments,
    [property: JsonPropertyName("auto_delete")] bool? AutoDelete,
    [property: JsonPropertyName("durable")] bool? Durable,
    [property: JsonPropertyName("internal")] bool? Internal,
    [property: JsonPropertyName("message_stats")] MessageStats MessageStats,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("user_who_performed_action")] string UserWhoPerformedAction,
    [property: JsonPropertyName("vhost")] string Vhost
);