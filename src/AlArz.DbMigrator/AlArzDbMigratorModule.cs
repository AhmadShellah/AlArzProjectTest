using AlArz.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace AlArz.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AlArzEntityFrameworkCoreModule),
    typeof(AlArzApplicationContractsModule)
    )]
public class AlArzDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
