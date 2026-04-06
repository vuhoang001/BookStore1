using BookStore.Catalog.Infrastructure.Blob;
using MediatR.Pipeline;

namespace BookStore.Catalog.Features.Book.Update;

public sealed class UpdateBookCommandPostProcessor(IBlobService blobService)
    : IRequestPostProcessor<UpdateBookCommand, Guid>
{
    public async Task Process(
        UpdateBookCommand request,
        Guid response,
        CancellationToken cancellationToken)
    {
        if (request is { IsRemoveImage: true, ImageUrn: not null })
        {
            await blobService.DeleteFileAsync(request.ImageUrn, cancellationToken);
        }
    }
}