namespace BankWebApp;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Description { get; set; }
    public DateTime Timestamp { get; set; }
}

public record OrderWrapper(Order Order, string Type);