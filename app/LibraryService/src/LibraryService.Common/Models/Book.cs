using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace LibraryService.Common.Models;

public class Book
{
    public int Id { get; set; }
    public Guid BookUid { get; set; } // Для uuid используем Guid
    public string Name { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public BookCondition Condition { get; set; }

    // Связь один-ко-многим с таблицей LibraryBooks
    public List<LibraryBooks> LibraryBooks { get; set; }
}

/// <summary>
/// Состояние книги
/// </summary>
/// <value>Состояние книги</value>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookCondition
{
    [EnumMember(Value = "EXCELLENT")]
    EXCELLENT = 0,
    
    [EnumMember(Value = "GOOD")]
    GOOD = 1,
    
    [EnumMember(Value = "BAD")]
    BAD = 2,
}