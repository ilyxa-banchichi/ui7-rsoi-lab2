using System.Net.Http.Json;
using System.Text.Json;
using Gateway.Common.Models.DTO;

namespace Gateway.Services;

public class LibraryService : ILibraryService
{
    private readonly HttpClient _httpClient;

    public LibraryService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://library-service:8060");
    }

    public async Task<LibraryPaginationResponse> GetLibrariesInCityAsync(
        string city, int? page, int? size)
    {
        var path = $"/api/v1/libraries?city={city}";
        if (page != null && size != null)
            path += $"&page={page}&size={size}";

        var response = await _httpClient.GetAsync(path);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(HttpRequestError.InvalidResponse,
                $"StatusCode: {response.StatusCode}");
            
        var result = await response.Content.ReadFromJsonAsync<LibraryPaginationResponse>();
        if (result == null)
            throw new JsonException("Invalid response");

        return result;
    }
}