using System.Threading.Tasks;

namespace StoreSample.Data;

public interface IStoreSampleDbSchemaMigrator
{
    Task MigrateAsync();
}