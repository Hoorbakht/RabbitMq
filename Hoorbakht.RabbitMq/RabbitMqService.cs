using System.Text;
using System.Text.Json;
using Hoorbakht.RabbitMq.Contracts;
using Hoorbakht.RabbitMq.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RestSharp;

namespace Hoorbakht.RabbitMq;

public class RabbitMqService : IRabbitMqService
{
	#region [Field(s)]

	private readonly RabbitMqConfiguration _configuration;

	public readonly ConnectionFactory ConnectionFactory;

	public readonly IModel Channel;

	public readonly IConnection Connection;

	#endregion

	#region [Constructor]

	public RabbitMqService(RabbitMqConfiguration rabbitMqConfiguration)
	{
		_configuration = rabbitMqConfiguration;
		ConnectionFactory = new ConnectionFactory
		{
			HostName = rabbitMqConfiguration.Host,
			UserName = rabbitMqConfiguration.Username,
			Password = rabbitMqConfiguration.Password,
			VirtualHost = rabbitMqConfiguration.VirtualHost,
			Port = rabbitMqConfiguration.Port

		};
		Connection = ConnectionFactory.CreateConnection();
		Channel = Connection.CreateModel();
	}

	#endregion

	#region [Method(s)]

	public void DeleteExchangeAndQueues(string exchangeName,
		List<string> queuesName, bool fireAndForget = false)
	{
		DeleteExchange(exchangeName, false, fireAndForget);
		foreach (var queue in queuesName)
			DeleteQueue(queue, false, fireAndForget);
	}

	public void DeleteAndAddExchangeAndQueues(ExchangeConfiguration exchangeConfiguration,
		List<QueueConfiguration> queueConfigurations, bool fireAndForget = false)
	{
		DeleteExchange(exchangeConfiguration.Name, false, fireAndForget);
		foreach (var queueConfiguration in queueConfigurations)
			DeleteQueue(queueConfiguration.Name!, false, fireAndForget);
		AddExchangeAndQueues(exchangeConfiguration, queueConfigurations, fireAndForget);
	}

	public void AddExchangeAndQueues(ExchangeConfiguration exchangeConfiguration, List<QueueConfiguration> queueConfigurations, bool fireAndForget = false)
	{
		AddExchange(exchangeConfiguration, fireAndForget);
		foreach (var queueConfiguration in queueConfigurations)
		{
			AddQueue(queueConfiguration, fireAndForget);
			if (queueConfiguration.BindConfigurations == null)
				BindQueueToExchange(
					new BindingConfiguration(exchangeConfiguration.Name, queueConfiguration.Name!),
					fireAndForget);
			else
				foreach (var item in queueConfiguration.BindConfigurations)
					BindQueueToExchange(
						new BindingConfiguration(exchangeConfiguration.Name,
							queueConfiguration.Name!, item.RoutingKey, item.Arguments),
						fireAndForget);
		}
	}

	public void BindQueueToExchange(BindingConfiguration bindingConfiguration, bool fireAndForget = false)
	{
		if (fireAndForget)
			Channel.QueueBindNoWait(bindingConfiguration.QueueName, bindingConfiguration.ExchangeName, bindingConfiguration.RoutingKey, bindingConfiguration.Arguments);
		else
			Channel.QueueBind(bindingConfiguration.QueueName, bindingConfiguration.ExchangeName, bindingConfiguration.RoutingKey, bindingConfiguration.Arguments);
	}

	public void AddQueue(QueueConfiguration queueConfiguration, bool fireAndForget = false)
	{
		if (fireAndForget)
			Channel.QueueDeclareNoWait(queueConfiguration.Name, queueConfiguration.Durable, queueConfiguration.Exclusive, queueConfiguration.AutoDelete, queueConfiguration.Arguments);
		else
			Channel.QueueDeclare(queueConfiguration.Name, queueConfiguration.Durable, queueConfiguration.Exclusive, queueConfiguration.AutoDelete, queueConfiguration.Arguments);
	}

	public void DeleteQueue(string name, bool ifUnused = false, bool fireAndForget = false)
	{
		if (fireAndForget)
			Channel.QueueDeleteNoWait(name, ifUnused);
		else
			Channel.QueueDelete(name, ifUnused);
	}

	public void AddExchange(ExchangeConfiguration exchangeConfiguration, bool fireAndForget = false)
	{
		if (fireAndForget)
			Channel.ExchangeDeclareNoWait(exchangeConfiguration.Name, exchangeConfiguration.ExchangeType, exchangeConfiguration.Durable, exchangeConfiguration.AutoDelete, exchangeConfiguration.Arguments);
		else
			Channel.ExchangeDeclare(exchangeConfiguration.Name, exchangeConfiguration.ExchangeType, exchangeConfiguration.Durable, exchangeConfiguration.AutoDelete, exchangeConfiguration.Arguments);
	}

	public void DeleteExchange(string name, bool ifUnused = false, bool fireAndForget = false)
	{
		if (fireAndForget)
			Channel.ExchangeDeleteNoWait(name, ifUnused);
		else
			Channel.ExchangeDelete(name, ifUnused);
	}

	public async Task<List<Exchange>?> GetAllExchangeAsync(CancellationToken cancellationToken = default)
	{
		var options = new RestClientOptions($"http://{_configuration.Host}:{_configuration.ManagementPort}")
		{
			MaxTimeout = -1
		};

		var client = new RestClient(options);
		var request = new RestRequest("/api/exchanges");

		request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(_configuration.Username + ":" + _configuration.Password)));

		var response = await client.ExecuteAsync<List<Exchange>>(request, cancellationToken);

		return response.Data;
	}

	public async Task<List<Queue>?> GetAllQueueAsync(CancellationToken cancellationToken = default)
	{
		var options = new RestClientOptions($"http://{_configuration.Host}:{_configuration.ManagementPort}")
		{
			MaxTimeout = -1
		};

		var client = new RestClient(options);
		var request = new RestRequest("/api/queues");

		request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(_configuration.Username + ":" + _configuration.Password)));

		var response = await client.ExecuteAsync<List<Queue>>(request, cancellationToken);

		return response.Data;
	}

	public async Task<List<User>?> GetAllUserAsync(CancellationToken cancellationToken = default)
	{
		var options = new RestClientOptions($"http://{_configuration.Host}:{_configuration.ManagementPort}")
		{
			MaxTimeout = -1
		};

		var client = new RestClient(options);
		var request = new RestRequest("/api/users");

		request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(_configuration.Username + ":" + _configuration.Password)));

		var response = await client.ExecuteAsync<List<User>>(request, cancellationToken);

		return response.Data;
	}

	public async Task<List<Binding>?> GetAllBindingAsync(CancellationToken cancellationToken = default)
	{
		var options = new RestClientOptions($"http://{_configuration.Host}:{_configuration.ManagementPort}")
		{
			MaxTimeout = -1
		};

		var client = new RestClient(options);
		var request = new RestRequest("/api/bindings");

		request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(_configuration.Username + ":" + _configuration.Password)));

		var response = await client.ExecuteAsync<List<Binding>>(request, cancellationToken);

		return response.Data;
	}

	public void Produce(ProduceModel model)
	{
		var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model.Model));

		Channel.BasicPublish(exchange: model.ExchangeName,
			routingKey: model.RoutingKey,
			basicProperties: model.BasicProperties,
			body: body);
	}
	public void CreateConsumer(ConsumerModel consumerModel)
	{
		var consumer = new EventingBasicConsumer(Channel);

		consumer.Received += consumerModel.Callback;

		Channel.BasicConsume(queue: consumerModel.QueueName,
			autoAck: consumerModel.AutoAck,
			consumer: consumer);
	}

	public void ConfirmMessage(ulong deliveryTag) =>
		Channel.BasicAck(deliveryTag, false);

	public void RejectMessage(ulong deliveryTag, bool requeue) =>
		Channel.BasicNack(deliveryTag, false, requeue);

	#endregion
}