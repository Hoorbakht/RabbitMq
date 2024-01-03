namespace Hoorbakht.RabbitMq.Models;

public record RabbitMqConfiguration(string? Host = "localhost", string? VirtualHost = "/", int Port = 5672, string? Username = "guest", string? Password = "guest", int ManagementPort = 15672);