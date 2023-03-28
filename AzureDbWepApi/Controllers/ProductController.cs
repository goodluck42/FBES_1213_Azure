using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzureDbWepApi.Controllers;

[ApiController]
[Route("api/v1")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ProductController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<Product[]> All()
    {
        return await _dbContext.Products.ToArrayAsync();
    }

    [HttpPost]
    public async Task Add(Product? product)
    {
        if (product is null)
        {
            return;
        }

        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
    }
}