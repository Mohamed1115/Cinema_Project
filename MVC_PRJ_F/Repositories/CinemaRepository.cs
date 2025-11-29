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

    public async Task<Cinema?> GetCinemaWithHallsAndMovies(int id)
    {
        return await _context.Cinemas
            .Include(a => a.Halls)
            .ThenInclude(m => m.Movies)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

}