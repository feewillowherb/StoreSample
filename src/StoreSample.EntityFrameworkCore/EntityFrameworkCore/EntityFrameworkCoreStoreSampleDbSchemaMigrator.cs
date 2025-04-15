using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreSample.Data;
using Volo.Abp.DependencyInjection;

namespace StoreSample.EntityFrameworkCore;

public class EntityFrameworkCoreStoreSampleDbSchemaMigrator
    : IStoreSampleDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreStoreSampleDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the StoreSampleDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<StoreSampleDbContext>()
            .Database
            .MigrateAsync();
    }
}
