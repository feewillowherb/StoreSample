using System.Diagnostics.Metrics;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using StoreSample.Assets;
using StoreSample.Orders;
using StoreSample.ValueObjects;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace StoreSample.Services;

[AutoConstructor]
[Authorize]
public partial class OrderService : ApplicationService
{
    private readonly IRepository<Order, OrderId> _orderRepository;
    private readonly IOrderManager _orderManager;
    
    public async Task<List<OrderDto>> GetListAsync()
    {
        var userId = UserId.From(CurrentUser.Id!.Value);

        var orders = await _orderRepository.GetListAsync(x => x.ForUser == userId);
        return orders.Select(x => new OrderDto
        {
            Id = x.Id,
            Status = x.Status,
            Amount = x.Amount,
            LineItems = x.LineItems.ToDictionary()
        }).ToList();
    }

    public async Task CreatePaymentAsync(CreatePaymentDto input)
    {
        var userId = UserId.From(CurrentUser.Id!.Value);
        await _orderManager.CreatePaymentAsync(userId, input.OrderId);
    }
}

public sealed record CreatePaymentDto
{
    public OrderId OrderId { get; set; }
}

public sealed record OrderDto
{
    public OrderId Id { get; set; }

    public OrderStatus Status { get; set; }

    public TokenValue Amount { get; set; }

    public Dictionary<ProductId, int> LineItems { get; set; } = new();
}