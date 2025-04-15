using MassTransit;
using StoreSample.Assets;
using StoreSample.Carts;
using StoreSample.Products;
using StoreSample.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace StoreSample.Orders;

public interface IOrderManager
{
    Task CreatePaymentAsync(UserId userId, OrderId orderId);

    Task CreateOrderAsync(UserId userId);
}

[AutoConstructor]
public partial class OrderManager : DomainService, IOrderManager
{
    private readonly IRepository<Order, OrderId> _orderRepository;
    private readonly IRepository<Asset, UserId> _assetRepository;
    private readonly IBus _publishEndpoint;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ICartManager _cartManager;
    private readonly IProductManager _productManager;
    private readonly IOrderMetricsManager _orderMetricsManager;

    public async Task CreatePaymentAsync(UserId userId, OrderId orderId)
    {
        var order = await _orderRepository.GetAsync(orderId);

        var asset = await _assetRepository.GetAsync(userId);

        if (userId != order.ForUser)
        {
            throw new UserFriendlyException("UserIdNotMatch");
        }

        if (order.Status != OrderStatus.Created)
        {
            throw new UserFriendlyException("OrderStatusNotCreated");
        }

        if (!asset.CheckBalance(order.Amount))
        {
            throw new UserFriendlyException("InsufficientBalance");
        }

        order.Pending();

        await _orderRepository.UpdateAsync(order);

        await _publishEndpoint.Publish(new CreatePayment(_guidGenerator.Create(), asset.Id, order.Id, order.Amount));

        await _orderMetricsManager.NewOrderAsync();
    }

    public async Task CreateOrderAsync(UserId userId)
    {
        var products = await _cartManager.GetAsync(userId);

        if (products.Count == 0)
        {
            throw new UserFriendlyException("CartIsEmpty");
        }

        var amount = await _productManager.GetAmountAsync(products);

        var order = new Order(
            OrderId.From(_guidGenerator.Create()),
            amount,
            userId,
            products);

        await _orderRepository.InsertAsync(order);
    }
}