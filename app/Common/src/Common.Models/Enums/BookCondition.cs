using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Common.Models.Enums;

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