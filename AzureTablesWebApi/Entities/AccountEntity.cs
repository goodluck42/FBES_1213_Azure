using Azure;
using Azure.Data.Tables;

namespace AzureTablesWebApi.Entities;

public class AccountEntity : ITableEntity
{
    #region ITableEntity
    public string PartitionKey
    {
        get => Country;
        set => Country = value;
    }
    public string RowKey { get; set; } = Guid.NewGuid().ToString();
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    #endregion

    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? Phone { get; set; }
}
