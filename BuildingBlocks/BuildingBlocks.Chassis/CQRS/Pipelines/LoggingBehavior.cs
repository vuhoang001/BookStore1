using System.Diagnostics;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Chassis.CQRS.Pipelines;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            logger.LogInformation(
                "[START] Handling {RequestName} | Payload: {Payload}",
                requestName,
                Serialize(request));

            var response = await next(cancellationToken);

            stopwatch.Stop();

            logger.LogInformation(
                "[END] Handled {RequestName} in {ElapsedMs}ms | Response: {Response}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                Serialize(response));

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            logger.LogError(
                ex,
                "[ERROR] Handling {RequestName} failed after {ElapsedMs}ms | Payload: {Payload}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                Serialize(request));

            throw;
        }
    }

    private static string Serialize(object? obj)
    {
        return obj is null
            ? string.Empty
            : JsonSerializer.Serialize(obj);
    }
}