using CosmosDbProject.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace CosmosDbProject.Controllers;

[ApiController]
[Route("api/v1/product")]
public class ProductController : Controller
{
    private readonly CosmosClient _client;
    private readonly Database _database;
    private readonly Container _container;

    public ProductController(CosmosClient client)
    {
        _client = client;
        _database = _client.GetDatabase("AppDb");
        _container = _database.GetContainer("ProductContainer");
    }

    [HttpGet("all")]
    public async Task<List<Product>> GetAll()
    {
        var queryDefinition = new QueryDefinition("SELECT * FROM c");
        var iterator = _container.GetItemQueryIterator<Product>(queryDefinition);
        var result = new List<Product>();

        while (iterator.HasMoreResults)
        {
            var products = await iterator.ReadNextAsync();

            result.AddRange(products);
        }

        return result;

    }

    [HttpGet]
    public async Task<Product?> GetProduct(string? id)
    {
        var queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id);
        var iterator = _container.GetItemQueryIterator<Product>(queryDefinition);

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();

            return response.SingleOrDefault();
        }

        return null;
    }

    [HttpPost]
    public async Task<Product?> CreateProduct(Product? product)
    {
        if (product == null)
        {
            return null;
        }

        product.Id = Guid.NewGuid().ToString();
        //var newProduct = product with { Id = Guid.NewGuid().ToString() };

        //var createdItem = await _container.CreateItemAsync(newProduct, new PartitionKey(newProduct.id));
        var createdItem = await _container.CreateItemAsync(product, new PartitionKey(product.Id));

        return createdItem;

    }

    [HttpDelete]
    public async Task DeleteProduct(string? id)
    {
        await _container.DeleteItemAsync<Product>(id, new PartitionKey(id));
    }

    [HttpPatch]
    public async Task UpdateProduct(Product? product)
    {
        if (product == null)
        {
            return;
        }

        await _container.ReplaceItemAsync<Product>(product, product.Id, new PartitionKey(product.Id));
        //await _container.UpsertItemAsync<Product>(product, new PartitionKey(product.Id));
    }

}
