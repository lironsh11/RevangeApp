using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RevengeApp.Models;

namespace RevengeApp.Data
{
    public class RevengeContext : IdentityDbContext
    {
        public RevengeContext(DbContextOptions<RevengeContext> options) : base(options) { }

        public DbSet<Revenge> Revenges { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Remove SQLite fallback - let DI handle it
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Critical for Identity!

            modelBuilder.Entity<Revenge>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Revenge>()
                .Property(r => r.Category)
                .HasConversion<string>();

            modelBuilder.Entity<Revenge>()
                .HasIndex(r => r.UserId);
        }
    }
}