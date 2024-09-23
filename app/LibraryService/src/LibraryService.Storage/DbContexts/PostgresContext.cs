using LibraryService.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Storage.DbContexts;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
    public DbSet<Library> Libraries { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<LibraryBooks> LibraryBooks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Связь один-ко-многим для LibraryBooks с Books и Libraries
        modelBuilder.Entity<LibraryBooks>()
            .HasKey(lb => new { lb.BookId, lb.LibraryId });

        modelBuilder.Entity<LibraryBooks>()
            .HasOne(lb => lb.Book)
            .WithMany(b => b.LibraryBooks)
            .HasForeignKey(lb => lb.BookId);

        modelBuilder.Entity<LibraryBooks>()
            .HasOne(lb => lb.Library)
            .WithMany(l => l.LibraryBooks)
            .HasForeignKey(lb => lb.LibraryId);

        modelBuilder.Entity<Book>()
            .Property(b => b.Condition)
            .HasConversion(
                v => v.ToString(), // Конвертация в строку при сохранении
                v => (BookCondition)Enum.Parse(typeof(BookCondition), v) // Конвертация из строки при чтении
            );
    }
}