using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp.Modularity;
using Volo.Abp.Users;

namespace StoreSample;

/* Inherit from this class for your domain layer tests. */
public abstract class StoreSampleDomainTestBase<TStartupModule> : StoreSampleTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
}