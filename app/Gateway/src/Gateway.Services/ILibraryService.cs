using Common.Models.DTO;

namespace Gateway.Services;

public interface ILibraryService
{
    Task<LibraryPaginationResponse> GetLibrariesInCityAsync(string city, int page, int size);
    Task<LibraryBookPaginationResponse> GetBooksInLibraryAsync(
        string libraryUid, int page, int size, bool showAll = false);

    Task<List<LibraryResponse>> GetLibrariesList(IEnumerable<Guid> librariesUid);
    Task<List<BookInfo>> GetBooksList(IEnumerable<Guid> booksUid);
}