using StoreSample.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace StoreSample.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(StoreSampleEntityFrameworkCoreModule)
)]
public class StoreSampleDbMigratorModule : AbpModule
{
}