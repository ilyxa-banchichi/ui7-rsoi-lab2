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
}