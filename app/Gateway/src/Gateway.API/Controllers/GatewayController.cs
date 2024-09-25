using System.ComponentModel.DataAnnotations;
using System.Net;
using Common.Models.DTO;
using Common.Models.Enums;
using Gateway.Common.Models.DTO;
using Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Controllers;

[Route("api/v1")]
[ApiController]
public class GatewayController(
    ILibraryService libraryService, IReservationService reservationService,
    IRatingService ratingService) : Controller
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
    public async Task<IActionResult> GetUserReservations([FromHeader(Name = "X-User-Name")][Required] string xUserName)
    {
        try
        {
            var rawReservations = await reservationService.GetUserReservationsAsync(xUserName);
            
            var booksUid = rawReservations.Select(r => r.BookUid);
            var librariesUid = rawReservations.Select(r => r.LibraryUid);

            var booksTask = libraryService.GetBooksListAsync(booksUid);
            var librariesTask = libraryService.GetLibrariesListAsync(librariesUid);

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
    
    /// <summary>
    /// Взять книгу в библиотеке
    /// </summary>
    /// <param name="xUserName">Имя пользователя</param>
    /// <param name="body"></param>
    /// <response code="200">Информация о бронировании</response>
    /// <response code="400">Ошибка валидации данных</response>
    [HttpPost("reservations")]
    [ProducesResponseType(typeof(TakeBookResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(List<BookReservationResponse>), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> TakeBook(
        [FromHeader(Name = "X-User-Name")][Required] string xUserName, [FromBody][Required] TakeBookRequest body)
    {
        try
        {
            var rawReservations = await reservationService.GetUserReservationsAsync(xUserName);
            var rentedCount = rawReservations.Count(r => r.Status == ReservationStatus.RENTED);
            
            var userRating = await ratingService.GetUserRating(xUserName);
            var maxRentedCount = Math.Ceiling((double)(userRating.Stars / 10));
            
            if (rentedCount > maxRentedCount)
                return Ok(null);
            
            var isBookTaken = await libraryService.TakeBookAsync(body.LibraryUid, body.BookUid);
            if (!isBookTaken)
                return Ok(null);

            var reservation = await reservationService.TakeBook(xUserName, body);

            var library = (await libraryService.GetLibrariesListAsync(new[] { body.LibraryUid }))[0];
            var book = (await libraryService.GetBooksListAsync(new[] { body.BookUid }))[0];
            
            var response = new TakeBookResponse()
            {
                ReservationUid = reservation.ReservationUid,
                Status = reservation.Status,
                StartDate = reservation.StartDate.ToString(),
                TillDate = reservation.TillDate.ToString(),
                Book = new BookInfo()
                {
                    BookUid = book.BookUid,
                    Name = book.Name,
                    Author = book.Author,
                    Genre = book.Genre
                },
                Library = library,
                Rating = userRating,
            };
            
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
    
    /// <summary>
    /// Получить рейтинг пользователя
    /// </summary>
    /// <param name="xUserName">Имя пользователя</param>
    /// <response code="200">Рейтинг пользователя</response>
    [HttpGet("rating")]
    [ProducesResponseType(typeof(UserRatingResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserRating([FromHeader(Name = "X-User-Name")][Required] string xUserName)
    { 
        try
        {
            var response = await ratingService.GetUserRating(xUserName);
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
}