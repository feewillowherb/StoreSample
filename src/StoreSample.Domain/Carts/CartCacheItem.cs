using StoreSample.ValueObjects;

namespace StoreSample.Carts;

public class CartCacheItem
{
    public UserId UserId { get; set; }

    public IDictionary<ProductId, int> Products { get; set; }
}