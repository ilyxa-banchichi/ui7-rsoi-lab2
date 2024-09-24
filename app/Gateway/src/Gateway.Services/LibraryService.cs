using System.Net.Http.Json;
using System.Text.Json;
using Gateway.Common.Models.DTO;

namespace Gateway.Services;

public class LibraryService : ILibraryService
{
    private readonly HttpClient _httpClient;

    public LibraryService(IHttpClientFactory httpClientFactory, string baseUrl)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<LibraryPaginationResponse> GetLibrariesInCityAsync(
        string city, int page, int size)
    {
        var path = $"/api/v1/libraries?city={city}&page={page}&size={size}";
        var response = await _httpClient.GetAsync(path);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(HttpRequestError.InvalidResponse,
                $"StatusCode: {response.StatusCode}");
            
        var result = await response.Content.ReadFromJsonAsync<LibraryPaginationResponse>();
        if (result == null)
            throw new JsonException("Invalid response");

        return result;
    }

    public async Task<LibraryBookPaginationResponse> GetBooksInLibraryAsync(
        string libraryUid, int page, int size, bool showAll = false)
    {
        var path = $"/api/v1/libraries/{libraryUid}/books?page={page}&size={size}&showAll={showAll}";
        var response = await _httpClient.GetAsync(path);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(HttpRequestError.InvalidResponse,
                $"StatusCode: {response.StatusCode}");
            
        var result = await response.Content.ReadFromJsonAsync<LibraryBookPaginationResponse>();
        if (result == null)
            throw new JsonException("Invalid response");

        return result;
    }
}