using MVC_PRJ_F.Data;

namespace MVC_PRJ_F.Models;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public int CinemaMovieId { get; set; }
    public CinemaMovies CinemaMovie { get; set; }
    public int Quantity { get; set; }
    public int TotalPrice { get; set; }
    
    public int? CouponId { get; set; }
    public Coupon? Coupon { get; set; }
}