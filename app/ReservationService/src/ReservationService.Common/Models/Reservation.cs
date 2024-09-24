using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ReservationService.Common.Models;

public class Reservation
{
    public int Id { get; set; }

    [Required]
    public Guid ReservationUid { get; set; }

    [Required]
    [MaxLength(80)]
    public string Username { get; set; }

    [Required]
    public Guid BookUid { get; set; }

    [Required]
    public Guid LibraryUid { get; set; }

    [Required]
    public ReservationStatus Status { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime TillDate { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReservationStatus
{
    [EnumMember(Value = "RENTED")]
    RENTED,
    
    [EnumMember(Value = "RETURNED")]
    RETURNED,
    
    [EnumMember(Value = "EXPIRED")]
    EXPIRED
}