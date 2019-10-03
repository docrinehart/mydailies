using Microsoft.EntityFrameworkCore;
using MyDailies.Data.Entities;

namespace MyDailies.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<DailyRating> DailyRatings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=test.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Metric>()
                .ToTable("kpi_metric")
                .HasKey(e => e.Id);

            modelBuilder.Entity<DailyRating>().HasKey(e => new { e.RatingDate, e.MetricId });
            modelBuilder.Entity<DailyRating>()
                .ToTable("kpi_dailies")
                .HasOne<Metric>(e => e.Metric)
                .WithMany(m => m.Ratings)
                .HasForeignKey(e => e.MetricId);
        }
    }
}
