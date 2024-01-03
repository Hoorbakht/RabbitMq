namespace Hoorbakht.RabbitMq.Models;

public record ExchangeConfiguration(string Name, string ExchangeType = ExchangeTypeConstants.FanOut, bool Durable = true, bool AutoDelete = false, IDictionary<string, object>? Arguments = null);