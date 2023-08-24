using AlArz.AdminManagement;
using Volo.Abp.Account;
using Volo.Abp.Auditing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace AlArz;

[DependsOn(
    typeof(AlArzDomainSharedModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpTenantManagementApplicationContractsModule),
    typeof(AbpObjectExtendingModule)
)]
public class AlArzApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AlArzDtoExtensions.Configure();
    }


}

public class CustomAuditLogContributor : AuditLogContributor
{
    public override void PostContribute(AuditLogContributionContext context)
    {
        var checkAdmin = new CheckAdmin();

        var publicIpAddressFromUser = checkAdmin.GetPublicIpAddress();

        var computerName = checkAdmin.GetComputerName();

        context.AuditInfo.Comments.Add($"computerName:{computerName},publicIpAddress:{publicIpAddressFromUser}");
    }


}