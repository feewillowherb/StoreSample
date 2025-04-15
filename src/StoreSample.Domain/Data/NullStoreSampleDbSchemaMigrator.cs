using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace StoreSample.Data;

/* This is used if database provider does't define
 * IStoreSampleDbSchemaMigrator implementation.
 */
public class NullStoreSampleDbSchemaMigrator : IStoreSampleDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}