namespace Hoorbakht.RabbitMq.Models;

public record BindConfiguration(string RoutingKey = "", IDictionary<string, object>? Arguments = null);