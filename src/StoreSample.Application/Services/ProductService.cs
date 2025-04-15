using Microsoft.AspNetCore.Authorization;
using StoreSample.Products;
using StoreSample.ValueObjects;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace StoreSample.Services;

[AutoConstructor]
[AllowAnonymous]
public partial class ProductService : ApplicationService
{
    private readonly IRepository<Product, ProductId> _repository;

    public async Task<List<ProductDto>> GetListAsync()
    {
        var products = await _repository.GetListAsync();
        return products.Select(x => new ProductDto
        {
            Id = x.Id,
            Name = x.Name,
            Cost = x.Cost
        }).ToList();
    }
}

public sealed record ProductDto
{
    public ProductId Id { get; set; }
    public string Name { get; set; }

    public TokenValue Cost { get; set; }
}