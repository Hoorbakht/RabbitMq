using Hoorbakht.RabbitMq.CommonSample.Constants;
using Hoorbakht.RabbitMq.CommonSample.ViewModels;
using Hoorbakht.RabbitMq.Contracts;
using Hoorbakht.RabbitMq.Models;

namespace Hoorbakht.RabbitMq.ProducerSample;

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