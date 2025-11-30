using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Repositories;

public class ActorRepository:Repository<Actor>, IActorRepository
{
    public ActorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Actor?> GetActorWithMovies(int id)
    {
        return await _context.Actors
            .Include(a => a.Movies)
            .ThenInclude(ma => ma.Movie)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}