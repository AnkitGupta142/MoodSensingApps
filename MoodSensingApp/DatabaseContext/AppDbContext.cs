using Microsoft.EntityFrameworkCore;
using MoodSensingApp.Models;


namespace MoodSensingApp.DatabaseContext
{

    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<MoodCapture> MoodCaptures { get; set; }
        public DbSet<Location> Locations { get; set; }

        public DbSet<MoodFrequency> MoodFrequencies { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity relationships, constraints, and other configurations

            modelBuilder.Entity<User>()
                .HasMany(u => u.MoodCaptures)
                .WithOne(mc => mc.User)
                .HasForeignKey(mc => mc.UserId);

            modelBuilder.Entity<User>()
                 .HasMany(u => u.MoodFrequencies)
                .WithOne(mf => mf.User)
                .HasForeignKey(mf => mf.UserId);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.MoodCaptures)
                .WithOne(mc => mc.Location)
                .HasForeignKey(mc => mc.LocationId);
        }
    }

}
