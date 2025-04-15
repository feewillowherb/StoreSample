using MassTransit;
using Volo.Abp.Uow;

namespace StoreSample.Filters;

[AutoConstructor]
public partial class UnitOfWorkFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        using var uow = _unitOfWorkManager.Begin();
        await next.Send(context); // Call the next middleware or handler

        await uow.CompleteAsync(); // Complete the Unit of Work
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("UnitOfWorkFilter");
    }
}