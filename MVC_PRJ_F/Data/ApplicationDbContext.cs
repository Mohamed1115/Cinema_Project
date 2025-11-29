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
    public DbSet<Cinema> Cinemas { get; set; }
    DbSet<MoveActor> MoveActors { get; set; }
    DbSet<MovImage> MovImages { get; set; }
    public DbSet<CinemaMovies> CinemaMovies { get; set; }
    public DbSet<Hall> Halls { get; set; }
    public DbSet<MoveCategory> MoveCategories { get; set; }

    public DbSet<Otp> Otps { get; set; }
    // DbSet<Movie> Movies { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Hall -> CinemaMovies (NO cascade)
        modelBuilder.Entity<CinemaMovies>()
            .HasOne(cm => cm.Hall)
            .WithMany(h => h.Movies)
            .HasForeignKey(cm => cm.HallId)
            .OnDelete(DeleteBehavior.Restrict);

        // Cinema -> CinemaMovies (NO cascade)
        modelBuilder.Entity<CinemaMovies>()
            .HasOne(cm => cm.Cinema)
            .WithMany(c => c.Movies)
            .HasForeignKey(cm => cm.CinemaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Cinema -> Halls (safe to keep Cascade)
        modelBuilder.Entity<Hall>()
            .HasOne(h => h.Cinema)
            .WithMany(c => c.Halls)
            .HasForeignKey(h => h.CinemaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}