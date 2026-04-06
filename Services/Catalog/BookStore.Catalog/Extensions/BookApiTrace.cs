using BookStore.Catalog.Domain.Events;
using BuildingBlocks.Chassis.Logging;

namespace BookStore.Catalog.Extensions;

internal static partial class BookApiTrace
{
    [LoggerMessage(
        EventId = 0,
        EventName = nameof(BookCreatedEvent),
        Level = LogLevel.Debug,
        Message = "Book with Id {BookId} created"
    )]
    public static partial void LogBookCreated(ILogger logger, [SensitiveData] Guid bookId);

    [LoggerMessage(
        EventId = 1,
        EventName = nameof(BookUpdateEvent),
        Level = LogLevel.Debug,
        Message = "Book with Id {BookId} updated"
    )]
    public static partial void LogBookUpdated(ILogger logger, [SensitiveData] Guid bookId);
}
