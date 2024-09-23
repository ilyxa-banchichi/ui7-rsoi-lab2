using System.Runtime.Serialization;

namespace LibraryService.Common.Models.DTO;

public class LibraryPaginationResponse
{
    /// <summary>
    /// Номер страницы
    /// </summary>
    /// <value>Номер страницы</value>
    [DataMember(Name="page")]
    public decimal? Page { get; set; }

    /// <summary>
    /// Количество элементов на странице
    /// </summary>
    /// <value>Количество элементов на странице</value>
    [DataMember(Name="pageSize")]
    public decimal? PageSize { get; set; }

    /// <summary>
    /// Общее количество элементов
    /// </summary>
    /// <value>Общее количество элементов</value>
    [DataMember(Name="totalElements")]
    public decimal? TotalElements { get; set; }

    /// <summary>
    /// Gets or Sets Items
    /// </summary>
    [DataMember(Name="items")]
    public List<LibraryResponse> Items { get; set; }
}