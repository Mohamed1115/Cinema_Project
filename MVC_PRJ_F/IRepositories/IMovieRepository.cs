using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.IRepositories;

public interface IMovieRepository:IRepository<Movie>
{
    Task<Movie?> GetMovieWithImage(int id);
}