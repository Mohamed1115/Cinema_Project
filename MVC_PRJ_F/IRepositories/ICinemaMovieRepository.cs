using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.IRepositories;

public interface ICinemaMovieRepository:IRepository<CinemaMovies>
{
    Task<List<CinemaMovies>> GetMovieWithHall(int id);
    Task<List<CinemaMovies>> GetAllByMovieId(int id);
}