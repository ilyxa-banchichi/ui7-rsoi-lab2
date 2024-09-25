using System.Net;
using Common.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using RatingService.Common.Converters;
using RatingService.Storage.Repositories;

namespace RatingService.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class RatingController(IRatingsRepository ratingsRepository) : Controller
{
    /// <summary>
    /// Получить рейтинг пользователя
    /// </summary>
    /// <param name="xUserName">Имя пользователя</param>
    /// <response code="200">Рейтинг пользователя</response>
    [HttpGet()]
    [ProducesResponseType(typeof(UserRatingResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserRating([FromHeader(Name = "X-User-Name")]string xUserName)
    { 
        try
        {
            var response = await ratingsRepository.GetUserRatingAsync(xUserName);
            if (response != null)
                return Ok(response.ConvertAppModelToDto());
            
            return Ok(null);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);
        }
    }
}