namespace MVC_PRJ_F.Models;

public class CinemaMovies
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int CinemaId { get; set; }
    public int HallId { get; set; }
    public TimeSpan Date { get; set; }
}