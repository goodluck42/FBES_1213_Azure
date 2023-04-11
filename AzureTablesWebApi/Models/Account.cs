using Azure;

namespace AzureTablesWebApi.Models;

public class Account
{
    public string? PartitionKey { get; set; }
    public string? RowKey { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
}
