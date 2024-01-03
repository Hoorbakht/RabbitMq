using System.Text.Json.Serialization;

namespace Hoorbakht.RabbitMq.Models;

public record User(
    [property: JsonPropertyName("hashing_algorithm")] string HashingAlgorithm,
    [property: JsonPropertyName("limits")] Limits Limits,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("password_hash")] string PasswordHash,
    [property: JsonPropertyName("tags")] IReadOnlyList<string> Tags
);