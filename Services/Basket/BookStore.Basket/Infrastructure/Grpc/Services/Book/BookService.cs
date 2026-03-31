using Bookstore.Catalog.V1;

namespace BookStore.Basket.Infrastructure.Grpc.Services.Book;

public class BookService(BookGrpcService.BookGrpcServiceClient bookGrpcServiceClient) : IBookService
{
    public async Task<GetBookResponse?> GetBookByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await bookGrpcServiceClient.GetBookAsync(
            new GetBookRequest { Id = id },
            cancellationToken: cancellationToken
        );
    }
}