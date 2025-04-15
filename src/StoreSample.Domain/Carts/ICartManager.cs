using MassTransit;
using StoreSample.Orders;
using StoreSample.ValueObjects;
using Volo.Abp;
using Volo.Abp.Caching.Hybrid;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace StoreSample.Carts;

public interface ICartManager
{
    Task SubmitAsync(UserId forUser, IDictionary<ProductId, int> products);

    Task<IDictionary<ProductId, int>> GetAsync(UserId forUser);


    Task ClearAsync(UserId forUser);
}

[AutoConstructor]
public partial class CartManager : DomainService, ICartManager
{
    private readonly IHybridCache<IDictionary<ProductId, int>, UserId> _cache;

    public async Task SubmitAsync(UserId forUser, IDictionary<ProductId, int> products)
    {
        await _cache.SetAsync(forUser, products);
    }

    public async Task<IDictionary<ProductId, int>> GetAsync(UserId forUser)
    {
        var cart = await _cache.GetOrCreateAsync(forUser,
            () => Task.FromResult((IDictionary<ProductId, int>)new Dictionary<ProductId, int>()));

        return cart!;
    }

    public async Task ClearAsync(UserId forUser)
    {
        await _cache.RemoveAsync(forUser);
    }
}

public sealed record CartItem
{
    public Dictionary<ProductId, int> Products { get; set; } = new();
    public TokenValue TotalPrice { get; set; }
}