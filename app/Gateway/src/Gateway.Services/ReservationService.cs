using Common.Models.DTO;

namespace Gateway.Services;

public class ReservationService(IHttpClientFactory httpClientFactory, string baseUrl)
    : BaseHttpService(httpClientFactory, baseUrl), IReservationService
{
    public async Task<List<RawBookReservationResponse>?> GetUserReservationsAsync(string xUserName)
    {
        var method = $"/api/v1/reservations";
        return await GetAsync<List<RawBookReservationResponse>>(method,
            new Dictionary<string, string>()
            {
                { "xUserName", xUserName }
            });
    }
}