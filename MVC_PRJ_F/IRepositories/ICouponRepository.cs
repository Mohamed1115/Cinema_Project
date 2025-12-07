using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.IRepositories;

public interface ICouponRepository : IRepository<Coupon>
{
    Task<Coupon?> GetByCodeAsync(string code);
}
