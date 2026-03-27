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
        logger.LogInformation("[PRE] CreateAuthorCommandPreProcessor hit. ImageNull: {ImageNull}", request.Image is null);

        if (request.Image is null)
        {
            logger.LogInformation("[PRE] Skip upload because Image is null");
            return await next(cancellationToken);
        }

        var url = await blobService.UploadFileAsync(request.Image, cancellationToken);
        request.ImageName = url;

        logger.LogInformation("[PRE] Upload success. ImageName: {ImageName}", request.ImageName);

        return await next(cancellationToken);
    }
}