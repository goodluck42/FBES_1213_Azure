namespace CosmosDbProject.Models;

public class ContainerOptions
{
    public string DatabaseName { get; set; } = null!;
    public string ContainerName { get; set; } = null!;
    public int Throughput { get; set; }
    public string PartitionKey { get; set; } = null!;
}
