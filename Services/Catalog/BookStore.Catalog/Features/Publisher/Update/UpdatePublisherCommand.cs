using BookStore.Catalog.Domain.AggregateModels.PublisherModel;
using BookStore.Catalog.Exceptions.Errors;
using BuildingBlocks.Chassis.CQRS;
using BuildingBlocks.Chassis.Exceptions;

namespace BookStore.Catalog.Features.Publisher.Update;

public record UpdatePublisherCommand(Guid PublisherId, string Name) : ICommand;

public class UpdatePublisherHandler(IPublisherRepository publisherRepository)
    : ICommandHandler<UpdatePublisherCommand>
{
    public async Task Handle(UpdatePublisherCommand command, CancellationToken cancellationToken)
    {
        var publisher = await publisherRepository.GetByIdAsync(command.PublisherId, cancellationToken);

        if (publisher is null) throw new NotFoundException(PublisherError.PublisherNotFound);

        publisher.UpdateName(command.Name);
        await publisherRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}