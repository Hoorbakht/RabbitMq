using Hoorbakht.RabbitMq;
using Hoorbakht.RabbitMq.ConsumerSample;
using Hoorbakht.RabbitMq.Contracts;
using Hoorbakht.RabbitMq.Models;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IRabbitMqService>(_ => new RabbitMqService(new RabbitMqConfiguration()));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();
