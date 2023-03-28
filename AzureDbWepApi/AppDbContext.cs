using Microsoft.EntityFrameworkCore;

namespace AzureDbWepApi;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired();
            entity.Property(x => x.Quantity).HasMaxLength(100);
        });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> Products { get; set; }
}