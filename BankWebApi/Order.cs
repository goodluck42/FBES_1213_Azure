namespace BankWebApi;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Description { get; set; }
    public DateTime Timestamp { get; set; }
}