using Microsoft.AspNetCore.Authorization;
using StoreSample.Carts;
using StoreSample.Orders;
using StoreSample.Products;
using StoreSample.ValueObjects;
using Volo.Abp.Application.Services;

namespace StoreSample.Services;

[AutoConstructor]
[Authorize]
public partial class CartService : ApplicationService
{
    private readonly ICartManager _cartManager;
    private readonly IOrderManager _orderManager;
    private readonly IProductManager _productManager;


    public async Task SubmitAsync(CartInputDto input)
    {
        var userId = UserId.From(CurrentUser.Id!.Value);
        await _cartManager.SubmitAsync(UserId.From(CurrentUser.Id!.Value), input.Products);
        await _orderManager.CreateOrderAsync(userId);
    }

    public async Task<CartDto> GetCartAsync()
    {
        var products = await _cartManager.GetAsync(UserId.From(CurrentUser.Id!.Value));

        var totalPrice = await _productManager.GetAmountAsync(products);

        var cartDto = new CartDto
        {
            Products = products.ToDictionary(),
            TotalPrice = totalPrice
        };

        return cartDto;
    }
}

public sealed record CartInputDto
{
    public Dictionary<ProductId, int> Products { get; set; } = new();
}

public sealed record CartDto
{
    public Dictionary<ProductId, int> Products { get; set; } = new();
    public TokenValue TotalPrice { get; set; }
}