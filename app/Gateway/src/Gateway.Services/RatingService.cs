using Common.Models.DTO;

namespace Gateway.Services;

public class RatingService(IHttpClientFactory httpClientFactory, string baseUrl)
    : BaseHttpService(httpClientFactory, baseUrl), IRatingService
{
    public async Task<UserRatingResponse?> GetUserRating(string xUserName)
    {
        var method = $"/api/v1/rating";
        return await GetAsync<UserRatingResponse>(method,
            new Dictionary<string, string>()
            {
                { "X-User-Name", xUserName }
            });
    }
}