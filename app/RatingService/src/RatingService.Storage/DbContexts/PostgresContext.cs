using RatingService.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace RatingService.Storage.DbContexts;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
    public DbSet<Rating> Ratings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rating>()
            .HasKey(r => r.Id);
        //
        // modelBuilder.Entity<Rating>()
        //     .ToTable(t => t.HasCheckConstraint("\"Stars\"", "stars BETWEEN 0 AND 100"));

        modelBuilder.Entity<Rating>().HasData(
            new Rating()
            {
                Id = 1,
                Username = "Ilya",
                Stars = 87
            },
            new Rating()
            {
                Id = 2,
                Username = "Pavel",
                Stars = 10
            }
        );
    }
}