using Bookstore.Catalog.V1;
using Grpc.Core;

namespace BookStore.Catalog.Infrastructure.Grpc.Services.Book;

public sealed class BookGrpcServiceImpl : BookGrpcService.BookGrpcServiceBase
{
    public override async Task<GetBookResponse> GetBook(GetBookRequest request, ServerCallContext context)
    {
        return new GetBookResponse
        {
            Book = new Bookstore.Catalog.V1.Book
            {
                Id          = Guid.NewGuid().ToString(),
                Title       = "New Book Title",
                Isbn        = "1234567890",
                Description = "A fascinating new book about gRPC services in .NET.",
                AuthorId    = Guid.NewGuid().ToString(),
                ImageUrl    = "https://example.com/images/new-book.jpg",
                IsAvailable = true
            }
        };
    }
}