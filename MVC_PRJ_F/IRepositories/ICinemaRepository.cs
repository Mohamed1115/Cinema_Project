using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.IRepositories;

public interface ICinemaRepository:IRepository<Cinema>
{
    Task<Cinema?> GetMovieWithHalls(int id);
    Task<Cinema?> GetCinemaWithMovies(int id);
}