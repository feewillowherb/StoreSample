using MassTransit;
using StoreSample.Orders;
using StoreSample.ValueObjects;

namespace StoreSample;

public record OrderCancelled(Guid CorrelationId, UserId ForUser, OrderId OrderId);

public record OrderPaymentSucceeeded(Guid CorrelationId, UserId ForUser, OrderId OrderId, TokenValue Amount);

public record OrderPaymentFailed(
    Guid CorrelationId,
    UserId ForUser,
    OrderId OrderId,
    ValueObjects.TokenValue Amount,
    string Message);

public record OrderCompleted(Guid CorrelationId, UserId ForUser, OrderId OrderId);

public record CreatePayment(Guid CorrelationId, UserId ForUser, OrderId OrderId, TokenValue Amount);

public record IncPayment(Guid CorrelationId, UserId UserId);