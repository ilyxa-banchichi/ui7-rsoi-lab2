using System.Text.Json.Serialization;

namespace ReservationService.Common.Models.DTO;

public class BookReservationResponse
{
    public Guid ReservationUid { get; set; }
    public string Username { get; set; }
    public Guid BookUid { get; set; }
    public Guid LibraryUid { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReservationStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime TillDate { get; set; }
}