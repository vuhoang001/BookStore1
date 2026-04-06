using System.Diagnostics;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Chassis.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("[{Handler}] TryHandleAsync called with exception type: {ExceptionType}", nameof(GlobalExceptionHandler), exception?.GetType().Name);
        
        // Don't handle specific exception types - let their dedicated handlers do it
        if (exception is NotFoundException or ValidationException)
        {
            logger.LogInformation("[{Handler}] Exception is {ExceptionType}, delegating to specific handler", nameof(GlobalExceptionHandler), exception.GetType().Name);
            return false;
        }
        
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        logger.LogError(
            exception,
            "[{Handler}] Could not process a request on machine {MachineName}. TraceId: {TraceId}",
            nameof(GlobalExceptionHandler),
            Environment.MachineName,
            traceId
        );

        var (statusCode, title) = MapException(exception);

        await TypedResults
            .Problem(
                title: title,
                statusCode: statusCode,
                extensions: new Dictionary<string, object?> { { nameof(traceId), traceId } }
            )
            .ExecuteAsync(httpContext);

        return true;
    }


    private static (int statusCode, string title) MapException(Exception exception)
    {
        return exception switch
        {
            NotFoundException => (
                StatusCodes.Status404NotFound,
                exception.Message
            ),
            ArgumentOutOfRangeException or ArgumentNullException or ArgumentException => (
                StatusCodes.Status400BadRequest,
                exception.Message
            ),
            InvalidOperationException or NotSupportedException => (
                StatusCodes.Status409Conflict,
                exception.Message
            ),
            _ => (StatusCodes.Status500InternalServerError, "We made a mistake but we are on it!"),
        };
    }
}