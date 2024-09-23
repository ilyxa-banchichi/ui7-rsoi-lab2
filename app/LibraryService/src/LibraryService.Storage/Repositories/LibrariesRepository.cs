using LibraryService.Common.Models;
using LibraryService.Storage.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Storage.Repositories;

public interface ILibrariesRepository
{
    Task<int> GetTotalElementCountAsync();
    Task<List<Library>> GetLibrariesInCityAsync(string city, int page = 1, int size = Int32.MaxValue);
}

public class LibrariesRepository(PostgresContext db) : ILibrariesRepository
{
    public async Task<int> GetTotalElementCountAsync()
    {
        return await db.Libraries.CountAsync();
    }
    
    public async Task<List<Library>> GetLibrariesInCityAsync(string city, int page = 1, int size = Int32.MaxValue)
    {
        var libraries =  await db.Libraries
            .Where(l => l.City == city)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return libraries;
    }
}