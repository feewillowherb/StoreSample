using StoreSample.ValueObjects;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace StoreSample.Products;

[AutoConstructor]
public partial class ProductDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Product, ProductId> _repository;
    private readonly IGuidGenerator _guidGenerator;


    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _repository.GetCountAsync() > 0)
        {
            return;
        }

        var products = new Product[]
        {
            new(GetProductId(_guidGenerator.Create()), "Product 1", TokenValue.From(10.0m)),
            new(GetProductId(_guidGenerator.Create()), "Product 2", TokenValue.From(20.0m)),
            new(GetProductId(_guidGenerator.Create()), "Product 3", TokenValue.From(30.0m))
        };

        await _repository.InsertManyAsync(products);

        return;

        ProductId GetProductId(Guid productId) => ProductId.From(productId);
    }
}