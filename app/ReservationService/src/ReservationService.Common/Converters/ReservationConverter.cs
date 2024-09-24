using ReservationService.Common.Models;
using ReservationService.Common.Models.DTO;

namespace ReservationService.Common.Converters;

public static class ReservationConverter
{
    public static BookReservationResponse ConvertAppModelToDto(this Reservation reservation)
    {
        return new BookReservationResponse()
        {
            ReservationUid = reservation.ReservationUid,
            Username = reservation.Username,
            BookUid = reservation.BookUid,
            LibraryUid = reservation.LibraryUid,
            Status = reservation.Status,
            StartDate = reservation.StartDate,
            TillDate = reservation.TillDate
        };
    }
}