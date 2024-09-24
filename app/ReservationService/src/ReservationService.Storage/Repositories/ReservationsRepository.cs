using Microsoft.EntityFrameworkCore;
using ReservationService.Common.Models;
using ReservationService.Storage.DbContexts;

namespace ReservationService.Storage.Repositories;

public class ReservationsRepository(PostgresContext db) : IReservationsRepository
{
    public async Task<List<Reservation>> GetUserReservationsAsync(string userName)
    {
        return await db.Reservations.Where(r => r.Username == userName).ToListAsync();
    }
}