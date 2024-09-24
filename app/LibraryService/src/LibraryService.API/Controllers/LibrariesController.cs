using System.ComponentModel.DataAnnotations;
using System.Net;
using Common.Models.DTO;
using LibraryService.Common.Converters;
using LibraryService.Storage.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LibrariesController(
    ILibrariesRepository librariesRepository, IBooksRepository booksRepository) : Controller
{
    /// <summary>
    /// Получить список библиотек в городе
    /// </summary>
    /// <param name="city">Город</param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <response code="200">Список библиотек в городе</response>
    [HttpGet]
    [ProducesResponseType(typeof(LibraryPaginationResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetLibrariesInCity([Required]string city, int page = 1, [Range(1, 100)]int size = 1)
    {
        try
        {
            var count = await librariesRepository.GetTotalElementCountAsync();
            
            var libraries = await librariesRepository.GetLibrariesInCityAsync(city, page, size);
            var response = new LibraryPaginationResponse()
            {
                Page = page,
                PageSize = size,
                TotalElements = count,
                Items = libraries.Select(lib => lib.ConvertAppModelToDto()).ToList()
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
    
    /// <summary>
    /// Получить список книг в выбранной библиотеке
    /// </summary>
    /// <param name="libraryUid">UUID библиотеки</param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="showAll"></param>
    /// <response code="200">Список книг библиотеке</response>
    [HttpGet("{libraryUid}/books")]
    [ProducesResponseType(typeof(LibraryBookPaginationResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBooksInLibrary([Required]string libraryUid, 
        int page = 1, [Range(1, 100)]int size = 1, bool showAll = false)
    {
        try
        {
            var guid = Guid.Parse(libraryUid);
            var count = await librariesRepository.GetTotaBooksCountByLibraryAsync(guid);
            
            var libraryBooks = await librariesRepository.GetBooksInLibraryAsync(guid, page, size, showAll);
            var response = new LibraryBookPaginationResponse()
            {
                Page = page,
                PageSize = size,
                TotalElements = count,
                Items = libraryBooks.Select(libraryBook => libraryBook.ConvertAppModelToDto()).ToList()
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
    
    [HttpGet("list")]
    [ProducesResponseType(typeof(List<LibraryResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetLibrariesList([FromHeader] IEnumerable<Guid> librariesUid)
    {
        try
        {
            var libraries = await librariesRepository.GetLibrariesListAsync(librariesUid);
            return Ok(libraries.Select(lib => lib.ConvertAppModelToDto()).ToList());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
    
    [HttpGet("books/list")]
    [ProducesResponseType(typeof(List<BookInfo>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBooksList([FromHeader] IEnumerable<Guid> booksUid)
    {
        try
        {
            var books = await booksRepository.GetBooksListAsync(booksUid);
            return Ok(books.Select(book => book.ConvertAppModelToDto()).ToList());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
}