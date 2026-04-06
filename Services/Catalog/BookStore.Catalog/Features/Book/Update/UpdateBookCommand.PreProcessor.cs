using BookStore.Catalog.Infrastructure.Blob;
using MediatR;

namespace BookStore.Catalog.Features.Book.Update;

public class UpdateBookCommandProcessor(IBlobService blobService) : IPipelineBehavior<UpdateBookCommand, Guid>
{
    public async Task<Guid> Handle(UpdateBookCommand request, RequestHandlerDelegate<Guid> next,
        CancellationToken cancellationToken)
    {
        if (request.Image is not null)
        {
            var urn = await blobService.UploadFileAsync(request.Image, cancellationToken);
            request.ImageUrn = urn;
        }

        return await next(cancellationToken);
    }
}