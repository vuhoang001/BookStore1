using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Chassis.Exceptions;

public sealed class NotFoundException(string message) : Exception(message)
{
    public static NotFoundException For<T>(Guid id)
    {
        return For<T>(id.ToString());
    }

    public static NotFoundException For<T>(string id)
    {
        return new($"{typeof(T).Name} with id {id} not found.");
    }
}

public sealed class NotFoundExceptionHandler(
    ILogger<NotFoundExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("[{Handler}] TryHandleAsync called with exception type: {ExceptionType}", nameof(NotFoundExceptionHandler), exception?.GetType().Name);
        
        if (exception is not NotFoundException notFoundException)
        {
            logger.LogInformation("[{Handler}] Exception is not NotFoundException, returning false", nameof(NotFoundExceptionHandler));
            return false;
        }

        logger.LogWarning(
            exception,
            "[{Handler}] Not found exception occurred: {Message}",
            nameof(NotFoundExceptionHandler),
            notFoundException.Message
        );

        await TypedResults
            .Problem(
                title: "NOT_FOUND",
                detail: notFoundException.Message,
                statusCode: StatusCodes.Status404NotFound)
            .ExecuteAsync(httpContext);

        return true;
    }
}