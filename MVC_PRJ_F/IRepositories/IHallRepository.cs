using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.IRepositories;

public interface IHallRepository:IRepository<Hall>
{
    Task<Hall?> GetHallWithMov(int id);
    Task<Hall?> GetAllc(int cId);
}