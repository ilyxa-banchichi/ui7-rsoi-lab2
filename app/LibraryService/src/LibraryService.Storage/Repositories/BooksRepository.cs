using LibraryService.Common.Models;
using LibraryService.Storage.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Storage.Repositories;

public class BooksRepository(PostgresContext db) : IBooksRepository
{
    public async Task<List<Book>> GetBooksListAsync(IEnumerable<Guid> booksUid)
    {
        return await db.Books
            .Where(l => booksUid.Contains(l.BookUid))
            .ToListAsync();
    }
}