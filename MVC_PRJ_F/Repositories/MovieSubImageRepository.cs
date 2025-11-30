using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Repositories;

public class MovieSubImageRepository:Repository<MovImage>,IMovieSubImageRepository
{
    public MovieSubImageRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task CreateAsync(MovImage subImage)
    {
        await _context.MovImages.AddAsync(subImage);
        await _context.SaveChangesAsync();
    }

    public async Task<List<MovImage>> GetByMovieIdAsync(int movieId)
    {
        return await _context.MovImages
            .Where(x => x.MovieId == movieId)
            .ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var sub = await _context.MovImages.FindAsync(id);
        if (sub != null)
        {
            _context.MovImages.Remove(sub);
            await _context.SaveChangesAsync();
        }
    }
}