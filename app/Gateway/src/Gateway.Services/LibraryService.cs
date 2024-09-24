using Common.Models.DTO;

namespace Gateway.Services;

public class LibraryService(IHttpClientFactory httpClientFactory, string baseUrl)
    : BaseHttpService(httpClientFactory, baseUrl), ILibraryService
{
    public async Task<LibraryPaginationResponse> GetLibrariesInCityAsync(
        string city, int page, int size)
    {
        var method = $"/api/v1/libraries?city={city}&page={page}&size={size}";
        return await GetAsync<LibraryPaginationResponse>(method);
    }

    public async Task<LibraryBookPaginationResponse> GetBooksInLibraryAsync(
        string libraryUid, int page, int size, bool showAll = false)
    {
        var method = $"/api/v1/libraries/{libraryUid}/books?page={page}&size={size}&showAll={showAll}";
        return await GetAsync<LibraryBookPaginationResponse>(method);
    }

    public async Task<List<LibraryResponse>> GetLibrariesList(IEnumerable<Guid> librariesUid)
    {
        var method = $"/api/v1/libraries/list";
        return await GetAsync<List<LibraryResponse>>(method,
            new Dictionary<string, string>()
            {
                { "librariesUid", string.Join(", ", librariesUid) }
            });
    }

    public async Task<List<BookInfo>> GetBooksList(IEnumerable<Guid> booksUid)
    {
        var method = $"/api/v1/libraries/books/list";
        return await GetAsync<List<BookInfo>>(method,
            new Dictionary<string, string>()
            {
                { "booksUid", string.Join(", ", booksUid) }
            });
    }
}