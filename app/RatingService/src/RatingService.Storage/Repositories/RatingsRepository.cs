using Microsoft.EntityFrameworkCore;
using RatingService.Common.Models;
using RatingService.Storage.DbContexts;

namespace RatingService.Storage.Repositories;

public class RatingsRepository(PostgresContext db) : IRatingsRepository
{
    public async Task<Rating?> GetUserRatingAsync(string userName)
    {
        return await db.Ratings.FirstOrDefaultAsync(r => r.Username == userName);
    }
}