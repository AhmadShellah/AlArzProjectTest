using AlArz.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AlArz.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AlArzController : AbpControllerBase
{
    protected AlArzController()
    {
        LocalizationResource = typeof(AlArzResource);
    }
}
