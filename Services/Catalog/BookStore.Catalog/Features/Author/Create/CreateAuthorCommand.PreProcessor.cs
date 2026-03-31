namespace BookStore.Catalog.Features.Author.Create;

using Infrastructure.Blob;
using MediatR;

public sealed class CreateAuthorCommandPreProcessor(
    IBlobService blobService,
    ILogger<CreateAuthorCommandPreProcessor> logger)
    : IPipelineBehavior<CreateAuthorCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateAuthorCommand request,
        RequestHandlerDelegate<Guid> next,
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