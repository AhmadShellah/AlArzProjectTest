using AlArz.AdminManagement;
using AlArz.Services;
using Application.Service;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace AlArz;

[DependsOn(
    typeof(AlArzDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(AlArzApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class AlArzApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AlArzApplicationModule>();
        });

        var configuration = context.Services.GetConfiguration();

        var bathUrlForAttachment = configuration[Admin.Attachment];

        var bathLogFiles = configuration[Admin.LogPath];

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.Configure<AuthorityLogContainer>(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = bathLogFiles;
                });
            });

            options.Containers.Configure<AttachmentContainer>(container =>
            {
                container.UseFileSystem(fileSystem =>
                {
                    fileSystem.BasePath = bathUrlForAttachment;
                });
            });
        });
    }
}
