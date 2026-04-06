using BookStore.Catalog.Infrastructure.Blob;
using MediatR;

namespace BookStore.Catalog.Features.Book.Create;

public class CreateBookCommandProcessor(IBlobService blobService) : IPipelineBehavior<CreateBookCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookCommand request, RequestHandlerDelegate<Guid> next,
        CancellationToken cancellationToken)
    {
        if (request.Image is null)
        {
            return await next(cancellationToken);
        }

        var url = await blobService.UploadFileAsync(request.Image, cancellationToken);
        request.ImageName = url;


        return await next(cancellationToken);
    }
}