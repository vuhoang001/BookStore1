using Bookstore.Catalog.V1;

namespace BookStore.Basket.Infrastructure.Grpc.Services.Book;

public interface IBookService
{
    Task<GetBookResponse?> GetBookByIdAsync(string id, CancellationToken cancellationToken = default);
}
