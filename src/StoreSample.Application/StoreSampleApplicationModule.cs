using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace StoreSample;

[DependsOn(
    typeof(StoreSampleDomainModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
)]
public class StoreSampleApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<StoreSampleApplicationModule>(); });

        context.Services.AddScoped<SignInManager<IdentityUser>>();
    }
}