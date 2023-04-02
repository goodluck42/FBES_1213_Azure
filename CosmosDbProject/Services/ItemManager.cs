using Microsoft.Azure.Cosmos;

namespace CosmosDbProject.Services;

public class ItemManager
{
    private readonly CosmosClient _client;

    public ItemManager(CosmosClient client)
	{
        _client = client;
    }
}
