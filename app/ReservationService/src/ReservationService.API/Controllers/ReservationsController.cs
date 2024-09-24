using System.ComponentModel.DataAnnotations;
using System.Net;
using Common.Models.DTO;
using ReservationService.Storage.Repositories;
using Microsoft.AspNetCore.Mvc;
using ReservationService.Common.Converters;

namespace ReservationService.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ReservationsController(IReservationsRepository reservationsRepository) : Controller
{
    /// <summary>
    /// Получить информацию по всем взятым в прокат книгам пользователя
    /// </summary>
    /// <param name="xUserName">Имя пользователя</param>
    /// <response code="200">Информация по всем взятым в прокат книгам</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<RawBookReservationResponse>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserReservations([FromHeader][Required] string xUserName)
    {
        try
        {
            var reservations = await reservationsRepository.GetUserReservationsAsync(xUserName);
            return Ok(reservations.Select(r => r.ConvertAppModelToDto()).ToList());
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
    [HttpPost()]
    [ProducesResponseType(typeof(RawBookReservationResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> TakeBook(
        [FromHeader][Required] string xUserName, [FromBody][Required] TakeBookRequest body)
    {
        try
        {
            var reservations = await reservationsRepository.CreateReservationAsync(
                userName: xUserName,
                bookUid: body.BookUid,
                libraryUid: body.LibraryUid,
                tillDate: body.TillDate);
            
            return Ok(reservations);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
}