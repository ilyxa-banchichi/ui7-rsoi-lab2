using System.ComponentModel.DataAnnotations;
using System.Net;
using Common.Models.DTO;
using Gateway.Common.Models.DTO;
using Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Controllers;

[Route("api/v1")]
[ApiController]
public class GatewayController(
    ILibraryService libraryService, IReservationService reservationService) : Controller
{
    /// <summary>
    /// Получить список библиотек в городе
    /// </summary>
    /// <param name="city">Город</param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <response code="200">Список библиотек в городе</response>
    [HttpGet("libraries")]
    [ProducesResponseType(typeof(LibraryPaginationResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetLibrariesInCity([Required]string city, int page = 1, [Range(1, 100)]int size = 1)
    {
        try
        {
            var response = await libraryService.GetLibrariesInCityAsync(city, page, size);
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
    [HttpGet("libraries/{libraryUid}/books")]
    [ProducesResponseType(typeof(LibraryBookPaginationResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBooksInLibrary([Required]string libraryUid, 
        int page = 1, [Range(1, 100)]int size = 1, bool showAll = false)
    {
        try
        {
            var response = await libraryService.GetBooksInLibraryAsync(libraryUid, page, size, showAll);
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
    
    /// <summary>
    /// Получить информацию по всем взятым в прокат книгам пользователя
    /// </summary>
    /// <param name="xUserName">Имя пользователя</param>
    /// <response code="200">Информация по всем взятым в прокат книгам</response>
    [HttpGet("reservations")]
    [ProducesResponseType(typeof(List<BookReservationResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserReservations([FromHeader][Required] string xUserName)
    {
        try
        {
            var rawReservations = await reservationService.GetUserReservationsAsync(xUserName);
            
            var booksUid = rawReservations.Select(r => r.BookUid);
            var librariesUid = rawReservations.Select(r => r.LibraryUid);

            var booksTask = libraryService.GetBooksList(booksUid);
            var librariesTask = libraryService.GetLibrariesList(librariesUid);

            var books = await booksTask;
            var libraries = await librariesTask;
            
            var reservations = new List<BookReservationResponse>(rawReservations.Count);
            
            for (int i = 0; i < rawReservations.Count; i++)
            {
                var rawReservation = rawReservations[i];
                reservations.Add(new BookReservationResponse()
                {
                    ReservationUid = rawReservation.ReservationUid,
                    Status = rawReservation.Status,
                    StartDate = rawReservation.StartDate.ToString(),
                    TillDate = rawReservation.TillDate.ToString(),
                    Book = books[i],
                    Library = libraries[i]
                });
            }
            
            return Ok(reservations);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
}