using LibraryService.Common.Models;
using LibraryService.Common.Models.DTO;

namespace LibraryService.Common.Converters;

public static class LibraryConverter
{
    public static LibraryResponse ConvertAppModelToDto(this Library library)
    {
        return new LibraryResponse()
        {
            LibraryUid = library.LibraryUid.ToString(),
            Name = library.Name,
            Address = library.Address,
            City = library.City
        };
    }
    
    public static LibraryBookResponse ConvertAppModelToDto(this LibraryBooks libraryBooks)
    {
        return new LibraryBookResponse()
        {
            BookUid = libraryBooks.Book.BookUid.ToString(),
            Name = libraryBooks.Book.Name,
            Author = libraryBooks.Book.Author,
            Genre = libraryBooks.Book.Genre,
            Condition = libraryBooks.Book.Condition,
            AvailableCount = libraryBooks.AvailableCount
        };
    }
}