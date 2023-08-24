using Volo.Abp.Modularity;

namespace AlArz;

[DependsOn(
    typeof(AlArzApplicationModule),
    typeof(AlArzDomainTestModule)
    )]
public class AlArzApplicationTestModule : AbpModule
{

}
