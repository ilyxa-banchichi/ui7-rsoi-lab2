using Common.Models.Enums;
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
    
    public async Task<Reservation> CreateReservationAsync(
        string userName, Guid bookUid, Guid libraryUid, DateTime tillDate)
    {
        var reservation = new Reservation()
        {
            Id = 0,
            ReservationUid = Guid.NewGuid(),
            Username = userName,
            BookUid = bookUid,
            LibraryUid = libraryUid,
            Status = ReservationStatus.RENTED,
            StartDate = DateTime.Now.ToUniversalTime(),
            TillDate = tillDate.ToUniversalTime()
        };
        
        reservation = (await db.Reservations.AddAsync(reservation)).Entity;
        await db.SaveChangesAsync();
        return await Task.FromResult(reservation);
    }
}