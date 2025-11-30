using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.IRepositories;

public interface IMovieSubImageRepository:IRepository<MovImage>
{
    Task CreateAsync(MovImage subImage);
    Task<List<MovImage>> GetByMovieIdAsync(int movieId);
    Task DeleteAsync(int id);
}