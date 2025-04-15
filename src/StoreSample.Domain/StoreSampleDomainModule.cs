using MassTransit;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.Caching;
using Volo.Abp.OpenIddict;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;

namespace StoreSample;

[DependsOn(
    typeof(StoreSampleDomainSharedModule),
    typeof(AbpCachingModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpEmailingModule),
    typeof(AbpIdentityDomainModule),
    typeof(BlobStoringDatabaseDomainModule),
    typeof(AbpOpenIddictDomainModule)
)]
public class StoreSampleDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        
        EndpointConvention.Map<IncPayment>(new Uri("queue:Asset"));
        Configure<AbpMultiTenancyOptions>(options => { options.IsEnabled = false; });
    }
}