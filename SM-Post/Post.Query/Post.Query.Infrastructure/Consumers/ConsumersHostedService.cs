using Amazon.Runtime.Internal.Util;
using CQRS.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Post.Query.Infrastructure.Consumers;

public class ConsumersHostedService : IHostedService
{
    private readonly ILogger<ConsumersHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ConsumersHostedService(ILogger<ConsumersHostedService> logger, IServiceProvider serviceProvider)
    {
        _logger=logger;
        _serviceProvider=serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("EventConsumer running");

        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            var eventconsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
            Task.Run(() => eventconsumer.Consume(topic), cancellationToken);
        }
            return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("EventConsumer stopped");
        return Task.CompletedTask;
    }
}

