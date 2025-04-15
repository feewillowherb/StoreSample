using MassTransit;
using StoreSample.ValueObjects;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace StoreSample.Assets;

[AutoConstructor]
public partial class AssetConsumer : IConsumer<CreatePayment>, IConsumer<IncPayment>
{
    private readonly IAssetManager _assetManager;

    public async Task Consume(ConsumeContext<CreatePayment> context)
    {
        var message = context.Message;

        await _assetManager.PayAsync(message.ForUser, message.OrderId, message.Amount);
    }

    public async Task Consume(ConsumeContext<IncPayment> context)
    {
        await _assetManager.IncBalance(context.Message.UserId);
    }
}
