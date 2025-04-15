using System.Text.Json;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using StoreSample.Assets;
using StoreSample.Orders;
using StoreSample.Products;
using StoreSample.ValueObjects;
using Vogen;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace StoreSample.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ConnectionStringName("Default")]
public partial class StoreSampleDbContext :
    AbpDbContext<StoreSampleDbContext>,
    IIdentityDbContext
{
    public StoreSampleDbContext(DbContextOptions<StoreSampleDbContext> options) : base(options)
    {
    }

    /* Add DbSet properties for your Aggregate Roots / Entities here. */


    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }


    public DbSet<Order> Orders { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Asset> Assets { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureIdentity();
        builder.ConfigureStoreSample();
        builder.ConfigureOpenIddict();
        builder.AddInboxStateEntity();
        builder.AddOutboxMessageEntity();
        builder.AddOutboxStateEntity();


        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(StoreSampleConsts.DbTablePrefix + "YourEntities", StoreSampleConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }


    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.RegisterAllInVogenEfCoreConverters();
    }
}

public static class ModelBuilderExtensions
{
    public static void ConfigureStoreSample(this ModelBuilder builder)
    {
        builder.Entity<Asset>(t =>
        {
            t.ConfigureByConvention();
            t.HasKey(x => x.Id);
        });

        builder.Entity<Product>(t =>
        {
            t.ConfigureByConvention();
            t.HasKey(x => x.Id);
        });

        builder.Entity<Order>(t =>
        {
            t.ConfigureByConvention();
            t.HasKey(x => x.Id);
            t.Property(o => o.LineItems)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<IDictionary<ProductId, int>>(v, (JsonSerializerOptions)null))
                .HasColumnType("jsonb");
        });
    }
}

[EfCoreConverter<UserId>]
[EfCoreConverter<ProductId>]
[EfCoreConverter<TokenValue>]
[EfCoreConverter<OrderId>]
internal static partial class VogenEfCoreConverters;