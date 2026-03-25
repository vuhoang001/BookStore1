using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

namespace BuildingBlocks.Chassis.Exceptions;

public sealed class ValidationExceptionHandler(
    ILogger<ValidationExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        logger.LogError(
            validationException,
            "[{Handler}] Exception occurred: {Message}",
            nameof(ValidationExceptionHandler),
            validationException.Message
        );


        var errors = validationException
            .Errors.GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

        var problemDetails = new Dictionary<string, object?> 
        { 
            { "errors", errors } 
        };

        await TypedResults
            .Problem(
                title: "VALIDATION_ERROR",
                detail: "One or more validation errors occurred.",
                statusCode: StatusCodes.Status400BadRequest,
                extensions: problemDetails)
            .ExecuteAsync(httpContext);

        return true;
    }
}