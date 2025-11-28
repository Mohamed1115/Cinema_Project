using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.IRepositories;

public interface ICategoryRepository:IRepository<Category>
{
    Task<Category?> GetCategoryWithMovies(int id);
}