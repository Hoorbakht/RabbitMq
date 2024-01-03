using System.Text;
using System.Text.Json;
using Hoorbakht.RabbitMq.CommonSample.Constants;
using Hoorbakht.RabbitMq.CommonSample.ViewModels;
using Hoorbakht.RabbitMq.Contracts;
using Hoorbakht.RabbitMq.Models;

namespace Hoorbakht.RabbitMq.ConsumerSample;

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