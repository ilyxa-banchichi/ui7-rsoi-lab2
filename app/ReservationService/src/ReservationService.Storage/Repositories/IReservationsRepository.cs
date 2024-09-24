using ReservationService.Common.Models;

namespace ReservationService.Storage.Repositories;

public interface IReservationsRepository
{
    Task<List<Reservation>> GetUserReservationsAsync(string userName);
}