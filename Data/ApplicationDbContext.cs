using LandingPageBuilder.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LandingPageBuilder.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LandingPage> LandingPages { get; set; } = null!;
        public DbSet<PageComponent> PageComponents { get; set; } = null!;
        public DbSet<ComponentType> ComponentTypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // LandingPage configuration
            modelBuilder.Entity<LandingPage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Slug).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.HasIndex(e => e.UserId);
            });

            // PageComponent configuration
            modelBuilder.Entity<PageComponent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ComponentName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Order).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasForeignKey(e => e.LandingPageId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ComponentType configuration
            modelBuilder.Entity<ComponentType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Seed component types
            modelBuilder.Entity<ComponentType>().HasData(
                new ComponentType { Id = 1, Name = "Hero", DisplayName = "Hero Section", Description = "Large banner section with headline and CTA" },
                new ComponentType { Id = 2, Name = "Features", DisplayName = "Features Section", Description = "Display key features in a grid" },
                new ComponentType { Id = 3, Name = "Testimonials", DisplayName = "Testimonials Section", Description = "Customer testimonials and reviews" },
                new ComponentType { Id = 4, Name = "CallToAction", DisplayName = "Call to Action", Description = "Button with headline and description" },
                new ComponentType { Id = 5, Name = "TextContent", DisplayName = "Text Content", Description = "Rich text content section" },
                new ComponentType { Id = 6, Name = "Gallery", DisplayName = "Image Gallery", Description = "Image gallery with multiple images" },
                new ComponentType { Id = 7, Name = "Newsletter", DisplayName = "Newsletter Signup", Description = "Email subscription form" },
                new ComponentType { Id = 8, Name = "Pricing", DisplayName = "Pricing Table", Description = "Pricing plans comparison" }
            );
        }
    }
}
