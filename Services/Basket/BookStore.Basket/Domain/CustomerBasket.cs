using System.Text.Json.Serialization;
using BookStore.Basket.Infrastructure.Exceptions;
using BuildingBlocks.SharedKernel.SeedWork;

namespace BookStore.Basket.Domain;

[method: JsonConstructor]
public sealed class CustomerBasket() : AuditableEntity
{
    private readonly List<BasketItem> _basketItems = [];

    public CustomerBasket(string id, List<BasketItem> items)
        : this()
    {
        if (!Guid.TryParse(id, out var basketId) || basketId == Guid.Empty)
            throw new BasketDomainException("Customer ID must be a valid non-empty GUID.");

        if (items is null || items.Count == 0)
            throw new BasketDomainException("Basket must contain at least one item.");

        Id = basketId;
        _basketItems = [.. items];
    }

    public IReadOnlyCollection<BasketItem> Items => _basketItems.AsReadOnly();

    /// <summary>
    ///     Updates the customer basket by replacing all existing items with the provided items.
    /// </summary>
    /// <param name="requestItems">The list of basket items to replace the current items with.</param>
    /// <returns>The current instance of <see cref="CustomerBasket" /> with updated items.</returns>
    public CustomerBasket Update(List<BasketItem> requestItems)
    {
        if (requestItems is null || requestItems.Count == 0)
            throw new BasketDomainException("Basket must contain at least one item.");

        _basketItems.Clear();
        _basketItems.AddRange(requestItems);
        return this;
    }
}
