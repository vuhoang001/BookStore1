using BookStore.Basket.Domain;
using StackExchange.Redis;

namespace BookStore.Basket.Infrastructure.Repositories;

public class BasketRepository(ILogger<BasketRepository> logger, IConnectionMultiplexer redis ) : IBasketRepository
{
    public Task<CustomerBasket?> GetBasketAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteBasketAsync(string id)
    {
        throw new NotImplementedException();
    }
}