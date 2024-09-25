using System.Runtime.Serialization;

namespace Common.Models.DTO;

[DataContract]
public class TakeBookRequest
{
    /// <summary>
    /// UUID книги
    /// </summary>
    /// <value>UUID книги</value>
    [DataMember(Name="bookUid")]
    public Guid BookUid { get; set; }

    /// <summary>
    /// UUID библиотеки
    /// </summary>
    /// <value>UUID библиотеки</value>
    [DataMember(Name="libraryUid")]
    public Guid LibraryUid { get; set; }

    /// <summary>
    /// Дата окончания бронирования
    /// </summary>
    /// <value>Дата окончания бронирования</value>
    [DataMember(Name="tillDate")]
    public DateOnly TillDate { get; set; }
}