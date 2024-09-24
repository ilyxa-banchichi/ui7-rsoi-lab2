using LibraryService.Common.Exceptions;
using LibraryService.Common.Models;
using LibraryService.Storage.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Storage.Repositories;

public class LibrariesRepository(PostgresContext db) : ILibrariesRepository
{
    public async Task<int> GetTotalElementCountAsync()
    {
        return await db.Libraries.CountAsync();
    }

    public async Task<int> GetTotaBooksCountByLibraryAsync(Guid libraryUid)
    {
        var library = await db.Libraries
            .Include(l => l.LibraryBooks)
            .FirstOrDefaultAsync(l => l.LibraryUid == libraryUid);
        
        if (library == null)
            throw new NotFoundEntityByIdException($"Library guid: {libraryUid}");

        return library.LibraryBooks.Count;
    }

    public async Task<List<Library>> GetLibrariesInCityAsync(
        string city, int page = 1, int size = Int32.MaxValue)
    {
        return await db.Libraries
            .Where(l => l.City == city)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();;
    }

    public async Task<List<LibraryBooks>> GetBooksInLibraryAsync(
        Guid libraryUid, int page = 1, int size = Int32.MaxValue, bool showAll = false)
    {
        var library = await db.Libraries.FirstOrDefaultAsync(l => l.LibraryUid == libraryUid);
        if (library == null)
            throw new NotFoundEntityByIdException($"Library guid: {libraryUid}");
        
        var libraryBooksQuery = db.LibraryBooks
            .Include(lb => lb.Book)
            .Where(lb => lb.LibraryId == library.Id);

        if (!showAll)
            libraryBooksQuery = libraryBooksQuery.Where(lb => lb.AvailableCount > 0);
        
        return await libraryBooksQuery
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<List<Library>> GetLibrariesListAsync(IEnumerable<Guid> librariesUid)
    {
        return await db.Libraries
            .Where(l => librariesUid.Contains(l.LibraryUid))
            .ToListAsync();
    }
    
    public async Task<bool> TakeBookAsync(Guid libraryUid, Guid bookUid)
    {
        var book = await db.LibraryBooks
            .Include(l => l.Library)
            .Include(l => l.Book)
            .FirstOrDefaultAsync(l => 
                l.Library.LibraryUid == libraryUid && l.Book.BookUid == bookUid);

        if (book.AvailableCount <= 0)
            return await Task.FromResult(false);

        book.AvailableCount -= 1;
        await db.SaveChangesAsync();
        
        return await Task.FromResult(true);
    }
}