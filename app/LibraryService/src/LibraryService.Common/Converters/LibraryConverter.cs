using LibraryService.Common.Models;
using LibraryService.Common.Models.DTO;

namespace LibraryService.Common.Converters;

public static class LibraryConverter
{
    public static LibraryResponse ConvertAppModelToDto(this Library library)
    {
        return new LibraryResponse()
        {
            LibraryUid = library.LibraryUid,
            Name = library.Name,
            Address = library.Address,
            City = library.City
        };
    }
}