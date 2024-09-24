using System.Runtime.Serialization;
using Common.Models.DTO;
using Common.Models.Enums;

namespace Gateway.Common.Models.DTO;

public class BookReservationResponse
{
    /// <summary>
    /// UUID бронирования
    /// </summary>
    /// <value>UUID бронирования</value>
    [DataMember(Name="reservationUid")]
    public Guid ReservationUid { get; set; }

    /// <summary>
    /// Статус бронирования книги
    /// </summary>
    /// <value>Статус бронирования книги</value>
    [DataMember(Name="status")]
    public ReservationStatus Status { get; set; }

    /// <summary>
    /// Дата начала бронирования
    /// </summary>
    /// <value>Дата начала бронирования</value>
    [DataMember(Name="startDate")]
    public string StartDate { get; set; }

    /// <summary>
    /// Дата окончания бронирования
    /// </summary>
    /// <value>Дата окончания бронирования</value>
    [DataMember(Name="tillDate")]
    public string TillDate { get; set; }
    
    [DataMember(Name="book")]
    public BookInfo Book { get; set; }
    
    [DataMember(Name="library")]
    public LibraryResponse Library { get; set; }
}