using AlArz.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace AlArz;

[DependsOn(
    typeof(AlArzEntityFrameworkCoreTestModule)
    )]
public class AlArzDomainTestModule : AbpModule
{

}
