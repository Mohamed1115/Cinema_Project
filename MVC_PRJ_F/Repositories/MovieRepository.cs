using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Repositories;

public class MovieRepository:Repository<Movie>,IMovieRepository
{
    public MovieRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<Movie?> GetMovieWithImage(int id)
    {
        return await _context.Movies
            .Include(m => m.SubImages)
            .Include(m => m.Actors)
            .Include(m => m.Categories)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

}