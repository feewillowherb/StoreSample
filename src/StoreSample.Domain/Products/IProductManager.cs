using StoreSample.ValueObjects;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace StoreSample.Products;

public interface IProductManager
{
    Task<ValueObjects.TokenValue> GetAmountAsync(IDictionary<ProductId, int> products);
}

[AutoConstructor]
public partial class ProductManager : DomainService, IProductManager
{
    private readonly IRepository<Product, ProductId> _productRepository;

    public async Task<TokenValue> GetAmountAsync(IDictionary<ProductId, int> products)
    {
        var productIds = products.Keys.ToList();
        var productEntities = await _productRepository.GetListAsync(x => productIds.Contains(x.Id));
        var amount = productEntities.Sum(x => x.GetTotalCost(products[x.Id]).Value);
        return TokenValue.From(amount);
    }
}