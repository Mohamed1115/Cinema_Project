using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Repositories;

public class CartRepository:Repository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Cart>> GetByIdUserAsync(string userId)
    {
        return await _context.Carts
            .Include(c => c.CinemaMovie)
                .ThenInclude(cm => cm.Movie)
            .Include(c => c.CinemaMovie)
                .ThenInclude(cm => cm.Cinema)
            .Include(c => c.CinemaMovie)
                .ThenInclude(cm => cm.Hall)
            .Include(c => c.Coupon)
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }
    
    
    public async Task<Cart?> GetByMovieAndUserAsync(int movieId, string userId)
    {
        return await _context.Carts
            .FirstOrDefaultAsync(c => c.CinemaMovieId == movieId && c.UserId == userId);
    }

    public async Task DeleteByUserAsync(string userId)
    {
        await _context.Carts
            .Where(c => c.UserId == userId)
            .ExecuteDeleteAsync();
        return;
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}