namespace Hoorbakht.RabbitMq.Models;

public record QueueConfiguration(string? Name, bool Durable = true, bool Exclusive = false, bool AutoDelete = false, IDictionary<string, object>? Arguments = null, List<BindConfiguration>? BindConfigurations = null)
{
    public QueueConfiguration(string? name, BindConfiguration bindConfiguration, bool durable = true, bool exclusive = false, bool autoDelete = false, IDictionary<string, object>? arguments = null) :
        this(name, durable, exclusive, autoDelete, arguments, new List<BindConfiguration> { bindConfiguration })
    {
    }
}