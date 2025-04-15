using MassTransit;
using MassTransit.Observables;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;
using StoreSample.EntityFrameworkCore;
using StoreSample.HealthChecks;
using Microsoft.OpenApi.Models;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using StoreSample.Data;
using StoreSample.Filters;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc.Libs;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;

namespace StoreSample;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(StoreSampleApplicationModule),
    typeof(StoreSampleEntityFrameworkCoreModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpAccountWebOpenIddictModule)
)]
public class StoreSampleHttpApiHostModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("StoreSample");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
            {
                serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx",
                    configuration["AuthServer:CertificatePassPhrase"]!);
                serverBuilder.SetIssuer(new Uri(configuration["AuthServer:Authority"]!));
            });
        }
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults
            .AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }


    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpMvcLibsOptions>(options => { options.CheckLibs = false; });

        if (!configuration.GetValue<bool>("App:DisablePII"))
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = false;
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.LogCompleteSecurityArtifact = false;
        }

        if (!configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata"))
        {
            Configure<OpenIddictServerAspNetCoreOptions>(options =>
            {
                options.DisableTransportSecurityRequirement = true;
            });

            Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedProto;
            });
        }

        ConfigureAuthentication(context);
        ConfigureUrls(configuration);
        ConfigureConventionalControllers();
        ConfigureHealthChecks(context);
        ConfigureSwagger(context, configuration);
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);


        context.Services.AddOpenTelemetry()
            .ConfigureResource(res => res.AddService("StoreSample"))
            .WithMetrics(builder =>
            {
                builder.AddAspNetCoreInstrumentation();
                builder.AddOtlpExporter();
            })
            .WithTracing(builder =>
            {
                builder.AddEntityFrameworkCoreInstrumentation();
                builder.AddOtlpExporter();
            });


        context.Services.AddMassTransit(options =>
        {
            //options.SetKebabCaseEndpointNameFormatter();
            options.UsingRabbitMq((c, cfg) =>
            {
                var connectionString =
                    c.GetRequiredService<IConfiguration>().GetConnectionString("RabbitMQ");
                cfg.Host(connectionString);
                cfg.UseJsonSerializer();
                cfg.UseJsonDeserializer();
                cfg.ConfigureEndpoints(c);
            });
            options.AddConsumers(typeof(StoreSampleDomainModule).Assembly);
            options.AddEntityFrameworkOutbox<StoreSampleDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
            });
            options.AddConfigureEndpointsCallback((ctx, nameof, configurator) =>
            {
                configurator.UseEntityFrameworkOutbox<StoreSampleDbContext>(ctx);
                configurator.UseConsumeFilter(typeof(UnitOfWorkFilter<>), ctx);
            });
        });

        context.Services.AddBusObserver<BusObservable>();
        context.Services.AddReceiveEndpointObserver<ReceiveEndpointObservable>();
        context.Services.AddReceiveObserver<ReceiveObservable>();
        context.Services.AddConsumeObserver<ConsumeObservable>();
        context.Services.AddSendObserver<SendObservable>();
        context.Services.AddPublishObserver<PublishObservable>();
        context.Services.AddOpenTelemetry().WithMetrics((Action<MeterProviderBuilder>)(m => m.AddMeter("MassTransit")))
            .WithTracing((Action<TracerProviderBuilder>)(t => t.AddSource("MassTransit")));
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ??
                                                 Array.Empty<string>());
        });
    }


    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<StoreSampleDomainSharedModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}StoreSample.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<StoreSampleDomainModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}StoreSample.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<StoreSampleApplicationModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath,
                        $"..{Path.DirectorySeparatorChar}StoreSample.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(StoreSampleApplicationModule).Assembly);
            options.ConventionalControllers.Create(typeof(AbpAccountApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwagger(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOidc(
            configuration["AuthServer:Authority"]!,
            ["StoreSample"],
            [AbpSwaggerOidcFlows.AuthorizationCode],
            null,
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "StoreSample API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
#if DEBUG
        context.Services.AddCors(options =>
        {
            // add allow any cors policy
            options.AddPolicy("AllowAny", builder =>
            {
                builder.SetIsOriginAllowed(s => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
#endif
    }

    private void ConfigureHealthChecks(ServiceConfigurationContext context)
    {
        context.Services.AddStoreSampleHealthChecks();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();


        app.UseRouting();
        app.UseAbpSecurityHeaders();
        app.UseCors();
        app.UseAuthentication();

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();
        app.UseAbpOpenIddictValidation();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "StoreSample API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);

            options.OAuthScopes("StoreSample");
        });
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }

    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await base.OnPostApplicationInitializationAsync(context);
        await context.ServiceProvider.GetRequiredService<StoreSampleDbMigrationService>().MigrateAsync();
    }
}