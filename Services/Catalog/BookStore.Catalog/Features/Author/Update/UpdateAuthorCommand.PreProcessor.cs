using BookStore.Catalog.Infrastructure.Blob;
using MediatR;

namespace BookStore.Catalog.Features.Author.Update;

public class UpdateAuthorCommandPreProcessor(IBlobService blobService) : IPipelineBehavior<UpdateAuthorCommand, Guid>
{
    public async Task<Guid> Handle(UpdateAuthorCommand request, RequestHandlerDelegate<Guid> next,
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