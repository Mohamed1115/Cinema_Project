namespace MVC_PRJ_F.Models;

public class CinemaMovies
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }  // ← Made nullable (navigation property)
    public int CinemaId { get; set; }
    public Cinema? Cinema { get; set; }  // ← Made nullable (navigation property)
    
    public int Price { get; set; }

    public int HallId { get; set; }
    public Hall? Hall { get; set; }  // ← Made nullable (navigation property)
    public TimeOnly Time { get; set; }
}