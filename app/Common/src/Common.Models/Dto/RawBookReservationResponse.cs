using System.Text.Json.Serialization;
using Common.Models.Enums;

namespace Common.Models.DTO;

public class RawBookReservationResponse
{
    public Guid ReservationUid { get; set; }
    public string Username { get; set; }
    public Guid BookUid { get; set; }
    public Guid LibraryUid { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReservationStatus Status { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly TillDate { get; set; }
}