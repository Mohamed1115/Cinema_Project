// using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
        
    }

    public DbSet<Actor> Actors { get; set; }
    DbSet<Movie> Movies { get; set; }
    public DbSet<Category> Categories { get; set; }
    DbSet<Cinema> Cinemas { get; set; }
    DbSet<MoveActor> MoveActors { get; set; }
    DbSet<MovImage> MovImages { get; set; }

    public DbSet<Otp> Otps { get; set; }
    // DbSet<Movie> Movies { get; set; }
    
    
}