using StoreSample.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace StoreSample.Orders;

public class Order : AggregateRoot<OrderId>
{
    public Order(
        OrderId id,
        TokenValue amount,
        UserId forUser,
        IDictionary<ProductId, int> lineItems
    ) : base(id)
    {
        Amount = amount;
        ForUser = forUser;
        LineItems = lineItems;
        Status = OrderStatus.Created;
    }

    public TokenValue Amount { get; private set; }

    public OrderStatus Status { get; private set; }

    public UserId ForUser { get; private set; }

    public IDictionary<ProductId, int> LineItems { get; private set; }

    public void Paid() =>
        Status = OrderStatus.Paid;

    public void Cancel() =>
        Status = OrderStatus.Canceled;

    public void Fail() =>
        Status = OrderStatus.Failed;

    public void Pending() =>
        Status = OrderStatus.Pending;
}

public enum OrderStatus
{
    Created = 0,
    Paid = 1,
    Canceled = 3,
    Failed = 4,
    Pending = 5,
}