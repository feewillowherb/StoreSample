using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Studio;
using Volo.Abp.Users.EntityFrameworkCore;

namespace StoreSample.EntityFrameworkCore;

[DependsOn(
    typeof(StoreSampleDomainModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpUsersEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule)
)]
public class StoreSampleEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        StoreSampleEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<StoreSampleDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        if (AbpStudioAnalyzeHelper.IsInAnalyzeMode)
        {
            return;
        }

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also StoreSampleDbContextFactory for EF Core tooling. */

            options.UseNpgsql();
        });


        context.Services.ConfigureOpenTelemetryMeterProvider(meter => meter.AddNpgsqlInstrumentation());
        context.Services.ConfigureOpenTelemetryTracerProvider(tracer => tracer.AddNpgsql());
    }
}