using Common.Models.DTO;

namespace Gateway.Services;

public interface IReservationService
{
    Task<List<RawBookReservationResponse>?> GetUserReservationsAsync(string xUserName);
}