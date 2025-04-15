using System.Diagnostics.Metrics;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace StoreSample.Orders;

public interface IOrderMetricsManager
{
    Task NewOrderAsync();
}

public class OrderMetricsManager : IOrderMetricsManager, ISingletonDependency
{
    private readonly Counter<int> _counter;

    public OrderMetricsManager(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("StoreSample.Orders");
        _counter = meter.CreateCounter<int>("NewOrderCounter", "orders", "The number of new orders");
    }

    public Task NewOrderAsync()
    {
        _counter.Add(1);
        return Task.CompletedTask;
    }
}