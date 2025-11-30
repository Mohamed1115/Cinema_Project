using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Repositories;

public class HallRepository:Repository<Hall>, IHallRepository
{
    public HallRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Hall>> GetAllc(int cId)
    {
        return await _context.Halls
            .Where(a => a.CinemaId == cId)
            .ToListAsync();

    }
    
    // في HallRepository
    public async Task<Hall?> GetHallWithMov(int id)
    {
        return await _context.Halls
            .Include(h => h.CinemaMovies)              // ✅ صح، لأن الـ property اسمه Movies
            .ThenInclude(cm => cm.Movie)         // جلب الفيلم من CinemaMovies
            .FirstOrDefaultAsync(h => h.Id == id);
    }
    
    
    
    
    
    
}