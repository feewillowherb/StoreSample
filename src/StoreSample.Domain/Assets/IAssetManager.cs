using MassTransit;
using StoreSample.ValueObjects;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace StoreSample.Assets;

public interface IAssetManager
{
    Task CreateAsync(Guid userId, TokenValue balance);


    Task PayAsync(UserId userId, OrderId orderId, TokenValue amount);

    Task IncBalance(UserId userId);
}

[AutoConstructor]
public partial class AssetManager : DomainService, IAssetManager
{
    private readonly IRepository<Asset, UserId> _repository;
    private readonly IBus _publishEndpoint;
    private readonly IGuidGenerator _guidGenerator;

    public async Task CreateAsync(Guid userId, TokenValue balance)
    {
        var asset = new Asset(UserId.From(userId), balance);

        await _repository.InsertAsync(asset);
    }

    public async Task PayAsync(UserId userId, OrderId orderId, TokenValue amount)
    {
        var asset = await _repository.FindAsync(x => x.Id == userId);

        if (asset == null)
        {
            await _publishEndpoint.Publish(new OrderPaymentFailed(_guidGenerator.Create(), userId,
                orderId,
                amount,
                "Asset not found"));

            return;
        }

        if (!asset.CheckBalance(amount))
        {
            await _publishEndpoint.Publish(new OrderPaymentFailed(_guidGenerator.Create(), userId,
                orderId,
                amount,
                "Insufficient balance"));

            return;
        }

        asset.Pay(amount);
        await _repository.UpdateAsync(asset);

        await _publishEndpoint.Publish(new OrderPaymentSucceeeded(_guidGenerator.Create(), userId,
            orderId,
            amount));
    }

    public async Task IncBalance(UserId userId)
    {
        var asset = await _repository.FindAsync(x => x.Id == userId);

        if (asset == null)
        {
            throw new Exception("Asset not found");
        }

        asset.IncBalance();
        await _repository.UpdateAsync(asset);
    }
}