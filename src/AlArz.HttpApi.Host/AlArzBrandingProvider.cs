using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace AlArz;

[Dependency(ReplaceServices = true)]
public class AlArzBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "AlArz";
}
