using StoreSample.Localization;
using Volo.Abp.Application.Services;

namespace StoreSample;

/* Inherit your application services from this class.
 */
public abstract class StoreSampleAppService : ApplicationService
{
    protected StoreSampleAppService()
    {
        LocalizationResource = typeof(StoreSampleResource);
    }
}
