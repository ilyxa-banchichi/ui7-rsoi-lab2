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
        var libraries = await db.Libraries
            .Where(l => l.City == city)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return libraries;
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
}