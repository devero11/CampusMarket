using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CampusMarket.Models;

namespace CampusMarket.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<FavoriteItem> FavoriteItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SellerRequest> SellerRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<FavoriteItem>()
                .HasIndex(f => new { f.UserId, f.AdId })
                .IsUnique();

            builder.Entity<CartItem>()
                .HasIndex(c => new { c.UserId, c.AdId })
                .IsUnique();


            builder.Entity<Category>()
                .HasMany(c => c.Ads)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CartItem>()
    .HasOne(c => c.User)
    .WithMany()
    .HasForeignKey(c => c.UserId)
    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FavoriteItem>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }
   
    }
}
