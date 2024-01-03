# RabbitMq
RabbitMQ Nuget to Connect RabbitMQ with RabbitMQ.Client

[Get Hoorbakht.RabbitMQ on nuget](https://www.nuget.org/packages/Hoorbakht.RabbitMQ/)


## Usage for .NET Core worker services

In this example, consider an app with a `SampleContract` entity. 
We'll use Hoorbakht.RabbitMQ to Produce and Consume SampleContract Entity with RabbitMQ Local Instance .

### 1. Add required services

Inject the `IRabbitMqService` service. So in `Program.cs` add:
```C#
builder.Services.AddSingleton<IRabbitMqService>(_ => new RabbitMqService(new RabbitMqConfiguration()));
```

### 2.(Producer) Get IRabbitMqService in Execute method and Use Methods

```C#
public class Worker(ILogger<Worker> logger, IRabbitMqService rabbitMqService) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		rabbitMqService.DeleteAndAddExchangeAndQueues(new ExchangeConfiguration("SampleExchange", ExchangeTypeConstants.Direct), new List<QueueConfiguration>
		{
			new(RabbitMqSampleConstants.FirstSampleQueueName,new BindConfiguration(RabbitMqSampleConstants.FirstSampleRoutingKey)),
			new(RabbitMqSampleConstants.SecondSampleQueueName,new BindConfiguration(RabbitMqSampleConstants.SecondSampleRoutingKey))
		}, true);

		while (!stoppingToken.IsCancellationRequested)
		{
			if (logger.IsEnabled(LogLevel.Information))
				logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");

			rabbitMqService.Produce(
				new ProduceModel(
					new SampleContract("Test",
						1,
						false,
						DateTime.Now),
					RabbitMqSampleConstants.SampleExchangeName,
					RabbitMqSampleConstants.FirstSampleRoutingKey));

			await Task.Delay(1000, stoppingToken);
		}
	}
}
```

### 2.(Consumer) Get IRabbitMqService in Execute method and Use Methods

```C#
public class Worker(ILogger<Worker> logger, IRabbitMqService rabbitMqService) : BackgroundService
{
	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		rabbitMqService.CreateConsumer(new ConsumerModel(RabbitMqSampleConstants.FirstSampleQueueName, false, (_, eventArgs) =>
		{
			var deliveryTag = eventArgs.DeliveryTag;

			var messageBody = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

			var message = JsonSerializer.Deserialize<SampleContract>(messageBody);

			logger.LogInformation(messageBody);

			rabbitMqService.ConfirmMessage(deliveryTag);
		}));
		return Task.CompletedTask;
	}
}
```
