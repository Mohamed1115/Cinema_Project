using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Repositories;

public class CategoryRepository:Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Category?> GetCategoryWithMovies(int id)
    {
        return await _context.Categories
            .Include(a => a.Movies)
            .ThenInclude(m => m.Movie)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}