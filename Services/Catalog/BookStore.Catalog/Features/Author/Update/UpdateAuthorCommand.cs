using System.Text.Json.Serialization;
using BookStore.Catalog.Domain.AggregateModels.AuthorModel;
using BookStore.Catalog.Exceptions.Errors;
using BuildingBlocks.Chassis.CQRS;
using BuildingBlocks.Chassis.Exceptions;

namespace BookStore.Catalog.Features.Author.Update;

public sealed class UpdateAuthorCommand : ICommand<Guid>
{
    public Guid AuthorId { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    public string? AuthorBio { get; set; }

    public IFormFile? Image { get; set; }

    [JsonIgnore] public string? ImageName { get; set; }
}

public class UpdateAuthorHandler(IAuthorRepository authorRepository)
    : ICommandHandler<UpdateAuthorCommand, Guid>
{
    public async Task<Guid> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = await authorRepository.GetByIdAsync(command.AuthorId, cancellationToken);

        if (author is null) throw new NotFoundException(AuthorError.AuthorNotFound);

        author.Update(command.AuthorName, command.AuthorBio, command.ImageName);

        await authorRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return author.Id;
    }
}