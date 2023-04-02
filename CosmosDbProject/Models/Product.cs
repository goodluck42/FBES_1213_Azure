using Newtonsoft.Json;

using System.Text.Json.Serialization;

namespace CosmosDbProject.Models;

//public record Product
//(
//    [JsonProperty("id")] string? Id,
//    [JsonProperty("name")] string? Name,
//    [JsonProperty("quantity")] int? Quantity,
//    [JsonProperty("price")] int? Price
//);

public class Product
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; } = null!;

    [JsonProperty("quantity")]
    public int? Quantity { get; set; }

    [JsonProperty("price")]
    public int? Price { get; set; }
}