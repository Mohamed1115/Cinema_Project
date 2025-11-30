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

    public async Task<Cinema?> GetMovieWithHalls(int id)
    {
        return await _context.Cinemas
            .Include(a => a.Movies)
            .ThenInclude(m => m.Hall)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    
    public async Task<Cinema?> GetCinemaWithMovies(int id)
    {
        var cinema = await _context.Cinemas
            .Include(c => c.Movies)
            .ThenInclude(cm => cm.Movie)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cinema != null)
        {
            // إزالة التكرارات بحسب MovieId
            cinema.Movies = cinema.Movies
                .GroupBy(cm => cm.MovieId)
                .Select(g => g.First())
                .ToList();
        }

        return cinema;
    }
    
    

}