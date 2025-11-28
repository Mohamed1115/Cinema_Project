using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.IRepositories;

public interface IActorRepository:IRepository<Actor>
{
    Task<Actor?> GetActorWithMovies(int id);
}