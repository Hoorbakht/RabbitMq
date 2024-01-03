using RabbitMQ.Client.Events;

namespace Hoorbakht.RabbitMq.Models;

public class ConsumerModel(string? queueName, bool autoAck, EventHandler<BasicDeliverEventArgs> callback)
{
    public string? QueueName { get; set; } = queueName;

    public bool AutoAck { get; set; } = autoAck;

    public EventHandler<BasicDeliverEventArgs> Callback { get; set; } = callback;
}