using Microsoft.AspNetCore.Identity;

namespace MVC_PRJ_F.Data;

public class ApplicationUser:IdentityUser
{
    public string Name { get; set; }
    public int? TotalCarts{ get; set; } =0;

    // public string City { get; set; }
    // public string Country { get; set; }
}