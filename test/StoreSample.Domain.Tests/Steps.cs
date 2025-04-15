using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using Shouldly;
using StoreSample.Assets;
using StoreSample.Carts;
using StoreSample.Orders;
using StoreSample.Products;
using StoreSample.Security;
using StoreSample.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace StoreSample;

[Binding]
public sealed partial class Steps : StoreSampleDomainTestBase<StoreSampleDomainTestModule>
{
    private DateTime _now;

    private Exception? _exception;

    private bool _isThrow = true;

    private TestManager M => GetRequiredService<TestManager>();


    [Given(@"Now is (.*)")]
    public void GivenNowIs(DateTime p0)
    {
        _now = p0;
    }

    [Given(@"Current user is ""(.*)""")]
    public void GivenCurrentUserIs(string p0)
    {
        FakeCurrentPrincipalAccessor.UserId = TestData.UserIdDict[p0];
    }

    [Given(@"Products as below")]
    public async Task GivenProductsAsBelow(Table table)
    {
        var infos = table.CreateSet<ProductDto>().ToList();

        var products = infos.Select(x => new Product(
            ProductId.From(Guid.NewGuid()),
            x.Name,
            TokenValue.From(x.Cost)
        )).ToList();

        await M.ProductRepository.InsertManyAsync(products);
    }

    [When(@"Create order as below")]
    public async Task WhenCreateOrderAsBelow(Table table)
    {
        var infos = table.CreateSet<CreateOrderInputDto>().ToList();

        var productNames = infos.Select(x => x.ProductName).Distinct().ToList();

        var allProductDict =
            (await M.ProductRepository.GetListAsync(x => productNames.Contains(x.Name))).ToDictionary(x => x.Name,
                x => x.Id);

        var productDict = infos.GroupBy(x => x.UserName)
            .Select(userGroup => new
            {
                UserName = userGroup.Key,
                Products = userGroup
                    .GroupBy(product => product.ProductName)
                    .ToDictionary(productGroup =>
                        allProductDict[productGroup.Key], productGroup => productGroup.Sum(p => p.Quantity))
            })
            .ToDictionary(x => x.UserName, x => x.Products);


        foreach (var (key, value) in productDict)
        {
            var userId = UserId.From(TestData.UserIdDict[key]);
            try
            {
                await M.CartManager.SubmitAsync(userId, value);

                await M.OrderManager.CreateOrderAsync(userId);
            }
            catch (Exception e)
            {
                _exception = e;
                if (_isThrow)
                {
                    throw;
                }
            }
        }
    }

    [Then("Order as below")]
    public async Task ThenOrderAsBelow(Table table)
    {
        var infos = table.CreateSet<OrderVerifyDto>().ToList();

        var userOrderDict = infos.GroupBy(x => x.UserName)
            .Select(userGroup => new
            {
                UserName = userGroup.Key,
                Orders = userGroup.ToList()
            })
            .ToDictionary(x => UserId.From(TestData.UserIdDict[x.UserName]), x => x.Orders);

        foreach (var (key, value) in userOrderDict)
        {
            var orders = await M.OrderRepository.GetListAsync(x => x.ForUser == key);

            orders.Count.ShouldBe(value.Count);
            foreach (var order in orders)
            {
                var verifyOrder = value.FirstOrDefault(x => x.Amount == order.Amount && x.Status == order.Status);
                verifyOrder.ShouldNotBeNull();
                value.Remove(verifyOrder);
            }
        }
    }

    [Given("Not throwing an exception")]
    public void GivenNotThrowingAnException()
    {
        _isThrow = false;
    }

    [Given("Throwing an exception")]
    public void GivenThrowingAnException()
    {
        _isThrow = true;
    }

    [When("Create payment as below")]
    public async Task WhenCreatePaymentAsBelow(Reqnroll.Table table)
    {
        var infos = table.CreateSet<CreatePaymentInputDto>().ToList();

        foreach (var info in infos)
        {
            var userId = UserId.From(TestData.UserIdDict[info.UserName]);
            try
            {
                var orderId = (await M.OrderRepository.GetListAsync(x => x.ForUser == userId))
                    .Select(x => x.Id).FirstOrDefault();
                await M.OrderManager.CreatePaymentAsync(userId, orderId);
            }
            catch (Exception e)
            {
                _exception = e;
                if (_isThrow)
                {
                    throw;
                }
            }
        }
    }

    [Then("Exception is {string}")]
    public void ThenExceptionIs(string cartIsEmpty)
    {
        _exception.ShouldNotBeNull();
        _exception.ShouldBeOfType<UserFriendlyException>();
        var domainException = (UserFriendlyException)_exception;
        domainException.Message.ShouldBe(cartIsEmpty);
    }

    [Given("Assets as below")]
    public async Task GivenAssetsAsBelow(Reqnroll.Table table)
    {
        var infos = table.CreateSet<CreateAssetDto>().ToList();

        var assets = infos.Select(x => new Asset(
            UserId.From(TestData.UserIdDict[x.UserName]),
            TokenValue.From(x.Value)
        )).ToList();

        await M.AssetRepository.InsertManyAsync(assets);
    }

    [When("Run Consumer")]
    public async Task WhenRunConsumer()
    {
        await M.TestHarness.Consumed.Any();
    }

    [Given("Start harness")]
    public async Task GivenStartHarness()
    {
        await M.TestHarness.Start();
    }

    [Then("Asset as below")]
    public async Task ThenAssetAsBelow(Reqnroll.Table table)
    {
        var infos = table.CreateSet<CreateAssetDto>().ToList();


        var assetInputs = infos.Select(x => new Asset(
            UserId.From(TestData.UserIdDict[x.UserName]),
            TokenValue.From(x.Value)
        )).ToList();

        var assets = await M.AssetRepository.GetListAsync(x =>
            assetInputs.Select(y => y.Id).Contains(x.Id));

        foreach (var asset in assets)
        {
            var assetInput = assetInputs.FirstOrDefault(x => x.Id == asset.Id);
            assetInput.ShouldNotBeNull();
            assetInput.Balance.ShouldBe(asset.Balance);
            assetInputs.Remove(assetInput);
        }
    }
}

file record CreateAssetDto(string UserName, decimal Value);

file record ProductDto(string Name, decimal Cost);

file record CreateOrderInputDto(string UserName, string ProductName, int Quantity);

file record OrderVerifyDto(
    string UserName,
    decimal Amount,
    OrderStatus Status
);

file record CreatePaymentInputDto(string UserName);

[AutoConstructor]
internal sealed partial class TestManager
{
    [field: AutoConstructorInject] public ITestHarness TestHarness { get; }
    [field: AutoConstructorInject] public IRepository<Asset, UserId> AssetRepository { get; }
    [field: AutoConstructorInject] public IRepository<Product, ProductId> ProductRepository { get; }

    [field: AutoConstructorInject] public IRepository<Order, OrderId> OrderRepository { get; }

    [field: AutoConstructorInject] public ICartManager CartManager { get; }

    [field: AutoConstructorInject] public IOrderManager OrderManager { get; }
}