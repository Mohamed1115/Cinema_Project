using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.IRepositories;

public interface ICartRepository:IRepository<Cart>
{
    Task<List<Cart>> GetByIdUserAsync(string userId);
    Task<Cart?> GetByMovieAndUserAsync(int movieId, string userId);
    Task DeleteByUserAsync(string userId);

}