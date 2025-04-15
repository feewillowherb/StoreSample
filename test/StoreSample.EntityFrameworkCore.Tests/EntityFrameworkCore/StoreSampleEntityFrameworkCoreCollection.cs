using Xunit;

namespace StoreSample.EntityFrameworkCore;

[CollectionDefinition(StoreSampleTestConsts.CollectionDefinitionName)]
public class StoreSampleEntityFrameworkCoreCollection : ICollectionFixture<StoreSampleEntityFrameworkCoreFixture>
{

}
