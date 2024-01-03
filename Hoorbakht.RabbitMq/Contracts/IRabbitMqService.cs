using Hoorbakht.RabbitMq.Models;

namespace Hoorbakht.RabbitMq.Contracts;

public interface IRabbitMqService
{
	void DeleteExchangeAndQueues(string exchangeName, List<string> queuesName, bool fireAndForget = false);

	void DeleteAndAddExchangeAndQueues(ExchangeConfiguration exchangeConfiguration,
	    List<QueueConfiguration> queueConfigurations, bool fireAndForget = false);

	void AddExchangeAndQueues(ExchangeConfiguration exchangeConfiguration,
	    List<QueueConfiguration> queueConfigurations, bool fireAndForget = false);

	void BindQueueToExchange(BindingConfiguration bindingConfiguration, bool fireAndForget = false);

	void AddQueue(QueueConfiguration queueConfiguration, bool fireAndForget = false);

	void DeleteQueue(string name, bool ifUnused = false, bool fireAndForget = false);

	void AddExchange(ExchangeConfiguration exchangeConfiguration, bool fireAndForget = false);

	void DeleteExchange(string name, bool ifUnused = false, bool fireAndForget = false);

	Task<List<Exchange>?> GetAllExchangeAsync(CancellationToken cancellationToken = default);

	Task<List<Queue>?> GetAllQueueAsync(CancellationToken cancellationToken = default);

	Task<List<User>?> GetAllUserAsync(CancellationToken cancellationToken = default);

	Task<List<Binding>?> GetAllBindingAsync(CancellationToken cancellationToken = default);

	void Produce(ProduceModel model);

	void CreateConsumer(ConsumerModel consumerModel);

	void ConfirmMessage(ulong deliveryTag);

	void RejectMessage(ulong deliveryTag, bool requeue);
}