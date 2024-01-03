using Hoorbakht.RabbitMq;
using Hoorbakht.RabbitMq.Contracts;
using Hoorbakht.RabbitMq.Models;
using Hoorbakht.RabbitMq.ProducerSample;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IRabbitMqService>(_ => new RabbitMqService(new RabbitMqConfiguration()));

//var exchanges = await rabbitMqService.GetAllExchangeAsync();

//var queues = await rabbitMqService.GetAllQueueAsync();

//var users = await rabbitMqService.GetAllUserAsync();

//var bindings = await rabbitMqService.GetAllBindingAsync();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();