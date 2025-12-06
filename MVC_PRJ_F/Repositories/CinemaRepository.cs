using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Repositories;

public class CinemaRepository:Repository<Cinema>, ICinemaRepository
{
    public CinemaRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets cinema with all movies and their associated halls
    /// </summary>
    public async Task<Cinema?> GetMovieWithHalls(int id)
    {
        try
        {
            return await _context.Cinemas
                .Include(c => c.Movies)
                    .ThenInclude(cm => cm.Movie)
                .Include(c => c.Movies)
                    .ThenInclude(cm => cm.Hall)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        catch (Exception)
        {
            return null;
        }
    }
    
    /// <summary>
    /// Gets cinema with halls and unique movies (no duplicates)
    /// </summary>
    public async Task<Cinema?> GetCinemaWithMovies(int id)
    {
        try
        {
            var cinema = await _context.Cinemas
                .Include(c => c.Halls)
                .Include(c => c.Movies)
                    .ThenInclude(cm => cm.Movie)
                .Include(c => c.Movies)
                    .ThenInclude(cm => cm.Hall)
                .FirstOrDefaultAsync(c => c.Id == id);

            // Remove duplicate movies (group by MovieId and take first)
            if (cinema?.Movies != null)
            {
                cinema.Movies = cinema.Movies
                    .GroupBy(cm => cm.MovieId)
                    .Select(g => g.First())
                    .ToList();
            }

            return cinema;
        }
        catch (Exception)
        {
            return null;
        }
    }
}