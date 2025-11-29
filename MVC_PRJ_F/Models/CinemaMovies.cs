namespace MVC_PRJ_F.Models;

public class CinemaMovies
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public int CinemaId { get; set; }
    public Cinema Cinema { get; set; }

    public int HallId { get; set; }
    public Hall Hall { get; set; }
    public TimeSpan Date { get; set; }
}