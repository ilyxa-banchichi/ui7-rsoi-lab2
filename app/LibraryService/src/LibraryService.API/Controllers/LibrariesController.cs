using System.ComponentModel.DataAnnotations;
using System.Net;
using LibraryService.Common.Converters;
using LibraryService.Common.Models;
using LibraryService.Common.Models.DTO;
using LibraryService.Storage.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LibrariesController(ILibrariesRepository librariesRepository) : Controller
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
}