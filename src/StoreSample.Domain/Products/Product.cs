using StoreSample.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace StoreSample.Products;

public class Product : AggregateRoot<ProductId>
{
    public Product(ProductId id, string name, ValueObjects.TokenValue cost)
        : base(id)
    {
        Name = name;
        Cost = cost;
    }


    public string Name { get; private set; }

    public TokenValue Cost { get; private set; }


    public TokenValue GetTotalCost(int quantity)
    {
        return Cost * quantity;
    }
}