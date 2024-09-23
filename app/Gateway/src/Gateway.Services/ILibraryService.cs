using Gateway.Common.Models.DTO;

namespace Gateway.Services;

public interface ILibraryService
{
    Task<LibraryPaginationResponse> GetLibrariesInCityAsync(string city, int? page, int? size);
}