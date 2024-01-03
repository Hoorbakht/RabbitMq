using RabbitMQ.Client;

namespace Hoorbakht.RabbitMq.Models;

public record ProduceModel(object Model, string ExchangeName, string RoutingKey = "", IBasicProperties? BasicProperties = null);