using MassTransit;
using StoreSample.Products;
using StoreSample.ValueObjects;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace StoreSample.Orders;

[AutoConstructor]
public sealed partial class OrderConsumer : IConsumer<OrderCancelled>,
    IConsumer<OrderPaymentSucceeeded>, IConsumer<OrderPaymentFailed>
{
    private readonly IRepository<Order, OrderId> _orderRepository;

    public async Task Consume(ConsumeContext<OrderCancelled> context)
    {
        var order = await _orderRepository.FindAsync(x => x.Id == context.Message.OrderId);
        if (order == null)
        {
            throw new Exception($"Order with ID {context.Message.OrderId} not found.");
        }

        order.Cancel();

        await _orderRepository.UpdateAsync(order);
    }

    public async Task Consume(ConsumeContext<OrderPaymentSucceeeded> context)
    {
        var order = await _orderRepository.FindAsync(x => x.Id == context.Message.OrderId);
        if (order == null)
        {
            throw new Exception($"Order with ID {context.Message.OrderId} not found.");
        }

        order.Paid();
        await _orderRepository.UpdateAsync(order);
    }

    public async Task Consume(ConsumeContext<OrderPaymentFailed> context)
    {
        var order = await _orderRepository.FindAsync(x => x.Id == context.Message.OrderId);
        if (order == null)
        {
            throw new Exception($"Order with ID {context.Message.OrderId} not found.");
        }

        order.Fail();
        await _orderRepository.UpdateAsync(order);
    }
}
