using BuildingBlocks.Constants.Core;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Chassis.EventBus;

public static class Extensions
{
    public static void AddEventBus(
        this IHostApplicationBuilder builder,
        Type type,
        Action<IBusRegistrationConfigurator>? busConfigure = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? rabbitMqConfigure = null
    )
    {
        var connectionString = builder.Configuration.GetConnectionString(Components.Queue);
        if (connectionString is null)
        {
            throw new InvalidOperationException("RabbitMQ connection string is not configured. Please ensure it is set in the configuration.");
        }

        builder.Services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumers(type.Assembly);

            config.AddActivities(type.Assembly);

            config.AddRequestClient(type);

            busConfigure?.Invoke(config);

            config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(connectionString));
                    configurator.UseMessageRetry(AddRetryConfiguration);
                    rabbitMqConfigure?.Invoke(context, configurator);

                    configurator.ConfigureEndpoints(context);
                }
            );
        });
    }

    private static void AddRetryConfiguration(IRetryConfigurator retryConfigurator)
    {
        retryConfigurator
            .Exponential(
                3,
                TimeSpan.FromMilliseconds(200),
                TimeSpan.FromMinutes(120),
                TimeSpan.FromMilliseconds(200)
            )
            .Ignore<ValidationException>();
    }
}