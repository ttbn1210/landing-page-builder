using Microsoft.EntityFrameworkCore;
using QrCommerce.Shared.Models;

namespace QrCommerce.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<QrLocation> QrLocations => Set<QrLocation>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<TelecomDevice> TelecomDevices => Set<TelecomDevice>();
    public DbSet<TelecomJob> TelecomJobs => Set<TelecomJob>();
    public DbSet<TelecomLog> TelecomLogs => Set<TelecomLog>();
    public DbSet<QrScanLog> QrScanLogs => Set<QrScanLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Campaign>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.CampaignCode).IsUnique();
            e.Property(x => x.Budget).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<QrLocation>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.QrCode).IsUnique();
            e.HasOne(x => x.Campaign).WithMany(c => c.QrLocations).HasForeignKey(x => x.CampaignId);
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Phone);
            e.Property(x => x.TotalSpent).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Price).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.OrderCode).IsUnique();
            e.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
            e.HasOne(x => x.Customer).WithMany(c => c.Orders).HasForeignKey(x => x.CustomerId);
            e.HasOne(x => x.QrLocation).WithMany(q => q.Orders).HasForeignKey(x => x.QrLocationId);
        });

        modelBuilder.Entity<OrderItem>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
            e.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
            e.HasOne(x => x.Order).WithMany(o => o.Items).HasForeignKey(x => x.OrderId);
            e.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        });

        modelBuilder.Entity<TelecomDevice>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.DeviceCode).IsUnique();
            e.HasIndex(x => x.ApiKey).IsUnique();
        });

        modelBuilder.Entity<TelecomJob>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Device).WithMany(d => d.Jobs).HasForeignKey(x => x.DeviceId);
        });

        modelBuilder.Entity<TelecomLog>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.TelecomJob).WithMany(j => j.Logs).HasForeignKey(x => x.TelecomJobId);
        });

        modelBuilder.Entity<QrScanLog>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.QrLocation).WithMany(q => q.ScanLogs).HasForeignKey(x => x.QrLocationId);
        });
    }
}
