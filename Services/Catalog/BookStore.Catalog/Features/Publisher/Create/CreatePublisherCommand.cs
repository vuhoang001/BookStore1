using BookStore.Catalog.Domain.AggregateModels.PublisherModel;
using BuildingBlocks.Chassis.CQRS;

namespace BookStore.Catalog.Features.Publisher.Create;

public sealed record CreatePublisherCommand(string Name) : ICommand<Guid>;

public sealed class CreatePublisherHandler(IPublisherRepository publisherRepository)
    : ICommandHandler<CreatePublisherCommand, Guid>
{
    public async Task<Guid> Handle(CreatePublisherCommand command, CancellationToken cancellationToken)
    {
        var result = await publisherRepository.AddAsync(
            new Catalog.Domain.AggregateModels.PublisherModel.Publisher(command.Name),
            cancellationToken);

        await publisherRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return result.Id;
    }
}