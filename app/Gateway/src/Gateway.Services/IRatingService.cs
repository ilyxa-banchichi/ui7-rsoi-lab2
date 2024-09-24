using Common.Models.DTO;

namespace Gateway.Services;

public interface IRatingService
{
    Task<UserRatingResponse?> GetUserRating(string xUserName);
}