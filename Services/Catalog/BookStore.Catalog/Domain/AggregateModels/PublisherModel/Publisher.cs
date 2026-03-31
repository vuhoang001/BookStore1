using BookStore.Catalog.Exceptions;
using BookStore.Catalog.Exceptions.Errors;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Catalog.Domain.AggregateModels.PublisherModel;

public class Publisher(string name) : Entity, IAggregateRoot
{
    public string? Name { get; private set; } = !string.IsNullOrWhiteSpace(name)
        ? name
        : throw new CatalogDomainException(PublisherError.PublisherNameRequired);


    public Publisher UpdateName(string name)
    {
        Name = !string.IsNullOrWhiteSpace(name)
            ? name
            : throw new CatalogDomainException("Publisher name must be provided.");
        return this;
    }
}