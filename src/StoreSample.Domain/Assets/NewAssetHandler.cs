using StoreSample.ValueObjects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;

namespace StoreSample.Assets;

[AutoConstructor]
public partial class NewAssetHandler : ILocalEventHandler<EntityCreatedEventData<IdentityUser>>, ITransientDependency
{
    private readonly IAssetManager _assetManager;

    public async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
    {
        await _assetManager.CreateAsync(eventData.Entity.Id, TokenValue.From(9999));
    }
}