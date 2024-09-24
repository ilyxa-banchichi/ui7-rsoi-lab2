using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Gateway.Services;

public abstract class BaseHttpService
{
    private readonly HttpClient _httpClient;

    protected BaseHttpService(IHttpClientFactory httpClientFactory, string baseUrl)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    protected async Task<T?> GetAsync<T>(string method, Dictionary<string, string>? headers = default)
    {
        _httpClient.DefaultRequestHeaders.Clear();

        if (headers != default)
        {
            foreach (var header in headers)
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
        
        var response = await _httpClient.GetAsync(method);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(HttpRequestError.InvalidResponse,
                $"StatusCode: {response.StatusCode}");

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            var result = await response.Content.ReadFromJsonAsync<T>();
            if (result == null)
                throw new JsonException("Invalid response");
        
            return result;
        }

        return default;
    }
    
    protected async Task<T?> PostAsync<T>(
        string method, object? body = null, Dictionary<string, string>? headers = default)
    {
        _httpClient.DefaultRequestHeaders.Clear();
            
        if (headers != default)
        {
            foreach (var header in headers)
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        var response = await _httpClient.PostAsync(method, JsonContent.Create(body));
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(HttpRequestError.InvalidResponse,
                $"StatusCode: {response.StatusCode}");

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            var result = await response.Content.ReadFromJsonAsync<T>();
            if (result == null)
                throw new JsonException("Invalid response");
        
            return result;
        }

        return default;
    }
    
    protected async Task<T?> PatchAsync<T>(
        string method, object? body = null, Dictionary<string, string>? headers = default)
    {
        _httpClient.DefaultRequestHeaders.Clear();
            
        if (headers != default)
        {
            foreach (var header in headers)
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        var response = await _httpClient.PatchAsync(method, JsonContent.Create(body));
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(HttpRequestError.InvalidResponse,
                $"StatusCode: {response.StatusCode}");

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            var result = await response.Content.ReadFromJsonAsync<T>();
            if (result == null)
                throw new JsonException("Invalid response");
        
            return result;
        }

        return default;
    }
}