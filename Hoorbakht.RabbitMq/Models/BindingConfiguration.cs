namespace Hoorbakht.RabbitMq.Models;

public record BindingConfiguration(string ExchangeName, string QueueName, string RoutingKey = "", IDictionary<string, object>? Arguments = null);