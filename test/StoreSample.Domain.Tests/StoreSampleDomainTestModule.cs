using System.Diagnostics;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using StoreSample.Assets;
using StoreSample.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace StoreSample;

[DependsOn(
    typeof(StoreSampleDomainModule),
    typeof(StoreSampleTestBaseModule),
    typeof(StoreSampleEntityFrameworkCoreTestModule)
)]
public class StoreSampleDomainTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Trace.Listeners.Add(new ConsoleTraceListener());

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Information)
            .MinimumLevel.Override("Volo.Abp", Serilog.Events.LogEventLevel.Warning)
            .WriteTo.Console()
            .CreateLogger();

        context.Services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddSerilog(Log.Logger);
            logging.SetMinimumLevel(LogLevel.Trace);
        });


        context.Services.AddMassTransitTestHarness(x =>
        {
            x.AddConsumers(typeof(StoreSampleDomainModule).Assembly);
            x.UsingInMemory((c, cfg) =>
            {
                cfg.UseDelayedMessageScheduler();

                cfg.ConfigureEndpoints(c);
            });
        });

        context.Services.AddScoped<TestManager>();
    }


    public override async Task OnPostApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await base.OnPostApplicationInitializationAsync(context);
        var testHarness = context.ServiceProvider.GetTestHarness();
        await testHarness.Start();
    }
}

public static class TestData
{
    public static readonly Dictionary<string, Guid> UserIdDict;

    static TestData()
    {
        UserIdDict = new Dictionary<string, Guid>
        {
            { "user1", Guid.Parse("7d578163-0428-4490-baa3-ef1666a7a7c3") },
            { "user2", Guid.Parse("dbe385fc-0451-4186-be33-15c1279056fd") }
        };
    }
}