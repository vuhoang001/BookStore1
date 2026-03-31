using BookStore.Catalog.Domain.AggregateModels.PublisherModel;
using BookStore.Catalog.Exceptions.Errors;
using BuildingBlocks.Chassis.CQRS;
using BuildingBlocks.Chassis.Exceptions;

namespace BookStore.Catalog.Features.Publisher.Delete;

public sealed record DeletePublisherCommand(Guid PublisherId) : ICommand;

public sealed class DeletePublisherHandler(IPublisherRepository publisherRepository)
    : ICommandHandler<DeletePublisherCommand>
{
    public async Task Handle(DeletePublisherCommand command, CancellationToken cancellationToken)
    {
        var publisher = await publisherRepository.GetByIdAsync(command.PublisherId, cancellationToken);

        if (publisher is null) throw new NotFoundException(PublisherError.PublisherNameRequired);

        publisherRepository.Delete(publisher);
        await publisherRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}