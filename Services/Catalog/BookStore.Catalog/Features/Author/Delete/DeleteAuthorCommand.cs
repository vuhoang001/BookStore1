using BookStore.Catalog.Domain.AggregateModels.AuthorModel;
using BookStore.Catalog.Exceptions.Errors;
using BuildingBlocks.Chassis.CQRS;
using BuildingBlocks.Chassis.Exceptions;

namespace BookStore.Catalog.Features.Author.Delete;

public sealed record DeleteAuthorCommand(Guid AuthorId) : ICommand;

public sealed class DeleteAuthorHandler(IAuthorRepository authorRepository) : ICommandHandler<DeleteAuthorCommand>
{
    public async Task Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
    {
        var author = await authorRepository.GetByIdAsync(command.AuthorId, cancellationToken);

        if (author is null) throw new NotFoundException(AuthorError.AuthorNameRequired);

        authorRepository.Remove(author);
        await authorRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}