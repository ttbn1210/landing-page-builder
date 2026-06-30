using Microsoft.EntityFrameworkCore;
using TrackQR.Web.Models;

namespace TrackQR.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<Placement> Placements => Set<Placement>();
    public DbSet<LandingPage> LandingPages => Set<LandingPage>();
    public DbSet<QRCode> QRCodes => Set<QRCode>();
    public DbSet<VisitorSession> VisitorSessions => Set<VisitorSession>();
    public DbSet<ScanEvent> ScanEvents => Set<ScanEvent>();
    public DbSet<TrackingEvent> TrackingEvents => Set<TrackingEvent>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<LeadActivity> LeadActivities => Set<LeadActivity>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Global query filter for soft delete
        modelBuilder.Entity<Campaign>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Placement>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<LandingPage>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<QRCode>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<VisitorSession>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Lead>().HasQueryFilter(e => !e.IsDeleted);
    }

    public override int SaveChanges()
    {
        SetTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
