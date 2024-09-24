using Common.Models.Enums;
using ReservationService.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ReservationService.Storage.DbContexts;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Указать, как сохранять enum в базу данных
        modelBuilder.Entity<Reservation>()
            .Property(r => r.Status)
            .HasConversion<string>() // Сохраняем как строку
            .HasMaxLength(20); // Задаем максимальную длину

        // Опционально, можно установить ограничения на уровне базы данных
        modelBuilder.Entity<Reservation>()
            .ToTable(t => t.HasCheckConstraint("CHK_Reservation_Status", "\"Status\" IN ('RENTED', 'RETURNED', 'EXPIRED')"));

        modelBuilder.Entity<Reservation>().HasData(
            new Reservation()
            { 
                Id = 1,
                ReservationUid = Guid.Parse("95428f22-731a-4c1c-9940-6479b25a8ade"),
                Username = "Ilya",
                BookUid = Guid.Parse("f7cdc58f-2caf-4b15-9727-f89dcc629b27"),
                LibraryUid = Guid.Parse("83575e12-7ce0-48ee-9931-51919ff3c9ee"),
                Status = ReservationStatus.RENTED,
                StartDate = DateTime.Parse("2024-09-17 00:00:00+00").ToUniversalTime(),
                TillDate = DateTime.Parse("2024-09-28 00:00:00+00").ToUniversalTime()
            },
            new Reservation()
            { 
                Id = 2,
                ReservationUid = Guid.Parse("c085af6e-13bb-4c17-ba0b-408dd436eff7"),
                Username = "Ilya",
                BookUid = Guid.Parse("931984da-a1bf-4920-b0a1-3ba53b9e950c"),
                LibraryUid = Guid.Parse("15507b2f-8a04-4e59-b2a9-b4d9eb7f7df0"),
                Status = ReservationStatus.RENTED,
                StartDate = DateTime.Parse("2024-09-05 00:00:00+00").ToUniversalTime(),
                TillDate = DateTime.Parse("2024-09-30 00:00:00+00").ToUniversalTime()
            },
            new Reservation()
            { 
                Id = 3,
                ReservationUid = Guid.Parse("0b1ef17b-3e4a-437b-829b-a288af63b9d5"),
                Username = "Pavel",
                BookUid = Guid.Parse("f7cdc58f-2caf-4b15-9727-f89dcc629b27"),
                LibraryUid = Guid.Parse("83575e12-7ce0-48ee-9931-51919ff3c9ee"),
                Status = ReservationStatus.EXPIRED,
                StartDate = DateTime.Parse("2024-09-15 00:00:00+00").ToUniversalTime(),
                TillDate = DateTime.Parse("2024-09-22 00:00:00+00").ToUniversalTime()
            }
        );
    }
}