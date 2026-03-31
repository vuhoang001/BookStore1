using System.Text.Json.Serialization;
using BookStore.Catalog.Domain.AggregateModels.AuthorModel;
using BuildingBlocks.Chassis.CQRS;

namespace BookStore.Catalog.Features.Author.Create;

public sealed class CreateAuthorCommand : ICommand<Guid>
{
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorBio { get; set; }
    public IFormFile? Image { get; set; }

    [JsonIgnore] public string? ImageName { get; set; }
}

public class CreateAuthorHandler(IAuthorRepository authorRepository, ILogger<CreateAuthorHandler> logger)
    : ICommandHandler<CreateAuthorCommand, Guid>
{
    public async Task<Guid> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
    {
        var author =
            new Domain.AggregateModels.AuthorModel.Author(command.AuthorName, command.AuthorBio, command.ImageName);

        var result = await authorRepository.AddAsync(author, cancellationToken);
        await authorRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return author.Id;
    }
}