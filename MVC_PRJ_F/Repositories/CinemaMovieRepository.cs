using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Repositories;

public class CinemaMovieRepository:Repository<CinemaMovies>, ICinemaMovieRepository
{
    public CinemaMovieRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<List<CinemaMovies>> GetMovieWithHall(int id)
    {
        return await _context.CinemaMovies
            .Where(c => c.MovieId == id)
            .Include(a => a.Hall)
            .ToListAsync();

    }

    public async Task<List<CinemaMovies>> GetAllByMovieId(int id)
    {
        return await  _context.CinemaMovies
            .Where(c => c.CinemaId == id)
            .Where(c => c.MovieId == id)
            .Include(a => a.Hall)
            .ToListAsync();
    }
}