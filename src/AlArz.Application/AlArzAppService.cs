using AlArz.Localization;
using Volo.Abp.Application.Services;

namespace AlArz;

/* Inherit your application services from this class.
 */
public abstract class AlArzAppService : ApplicationService
{
    protected AlArzAppService()
    {
        LocalizationResource = typeof(AlArzResource);
    }
}
