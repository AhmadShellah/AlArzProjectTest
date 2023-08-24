using AlArz.AdminManagement.Services;
using AlArz.EntityFrameworkCore;
using AlArz.MultiTenancy;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace AlArz;

[DependsOn(
    typeof(AlArzHttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AlArzApplicationModule),
    typeof(AlArzEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpAccountWebIdentityServerModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)

)]
public class AlArzHttpApiHostModule : AbpModule
{
    string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    //public override void PreConfigureServices(ServiceConfigurationContext context)
    //{
    //    PreConfigure<OpenIddictBuilder>(builder =>
    //    {
    //        builder.AddValidation(options =>
    //        {
    //            options.AddAudiences("AlArz");
    //            options.UseLocalServer();
    //            options.UseAspNetCore();
    //        });
    //    });
    //}

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigureAuthentication(context, configuration);
        //ConfigureConventionalControllers();
        ConfigureLocalization();
        ConfigureVirtualFileSystem(context);

        ConfigureSwaggerServices(context, configuration);



        if (context.Services.GetHostingEnvironment().IsDevelopment())
        {
            context.Services.Configure<AbpExceptionHandlingOptions>(options =>
            {
                options.SendExceptionsDetailsToClients = true;

            });
        }



        context.Services.ConfigureApplicationCookie(options =>
        {
            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = ctx =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api"))
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }
                    return Task.FromResult(0);
                },
                OnRedirectToAccessDenied = ctx =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api"))
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }
                    return Task.FromResult(0);
                },
            };
        });

        context.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              builder =>
                              {
                                  builder
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowAnyOrigin()
                                  //.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                                  ;
                              });
        });


        Configure<AppUrlOptions>(options =>
        {
            var passwordResetUrl = configuration["PasswordResetUrl"];
            options.Applications["AAUP"].Urls[AccountUrlNames.PasswordReset] = passwordResetUrl;
        });

    }
    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {

            options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));

            options.Languages.Add(new LanguageInfo("en", "en", "English"));


        });

        //AbpAuditingOptions
        //Configure<AbpAuditingOptions>(options =>
        //{
        //    options.Contributors.Add(new MyAuditLogContributor());
        //});
    }


    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {

        context.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "AlArz";
                //options.BackchannelHttpHandler = new HttpClientHandler
                //{
                //    ServerCertificateCustomValidationCallback =
                //        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                //};
                options.MapInboundClaims = true;
            });

        context.Services.ForwardIdentityAuthenticationForBearer();

        //context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            //options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            //options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            //options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<AlArzDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AlArz.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<AlArzDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AlArz.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<AlArzApplicationContractsModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AlArz.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<AlArzApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}AlArz.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(AlArzApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
    {
        //context.Services.AddAbpSwaggerGenWithOAuth(
        //    configuration["AuthServer:Authority"],
        //    new Dictionary<string, string>
        //    {
        //            {"AlArz", "AlArz API"}
        //    },
        //    options =>
        //    {
        //        options.SwaggerDoc("v1", new OpenApiInfo { Title = "AlArz API", Version = "v1" });
        //        options.DocInclusionPredicate((docName, description) => true);
        //        options.CustomSchemaIds(type => type.FullName);
        //    });

        context.Services.AddTransient<ICustomTokenRequestValidator, DefaultClientClaimsAdder>();
        context.Services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AlArz API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                //---------------------------------------------------------------
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                //---------------------------------------------------------------

            });

    }

    //private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    //{
    //    context.Services.AddCors(options =>
    //    {
    //        //options.AddDefaultPolicy(builder =>
    //        //{
    //        //    builder
    //        //        .WithOrigins(configuration["App:CorsOrigins"]?
    //        //            .Split(",", StringSplitOptions.RemoveEmptyEntries)
    //        //            .Select(o => o.RemovePostFix("/"))
    //        //            .ToArray() ?? Array.Empty<string>())
    //        //        .WithAbpExposedHeaders()
    //        //        .SetIsOriginAllowedToAllowWildcardSubdomains()
    //        //        .AllowAnyHeader()
    //        //        .AllowAnyMethod()
    //        //        .AllowCredentials();
    //        //});

    //        options.AddPolicy(name: MyAllowSpecificOrigins,
    //              builder =>
    //              {
    //                  builder
    //                  .AllowAnyHeader()
    //                  .AllowAnyMethod()
    //                  .AllowAnyOrigin()
    //                  //.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
    //                  ;
    //              });

    //    });
    //}

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseAbpRequestLocalization();

        //if (!env.IsDevelopment())
        //{
        //    app.UseErrorPage();
        //}

        app.UseHttpLogging();//TODO:Logging

        app.UseCorrelationId();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors(MyAllowSpecificOrigins);
        app.UseAuthentication();
        app.UseJwtTokenMiddleware();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "AlArz API");
            //c.SwaggerEndpoint("/swagger/v1/swagger.json", "AlArz API");

            //var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            //c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            //c.OAuthScopes("AlArz");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
