namespace MVC_PRJ_F.Models;

public class Hall
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Capacity { get; set; }
    public string Type { get; set; }
    public int CinemaId { get; set; }
    public Cinema Cinema { get; set; }
    public List<CinemaMovies> Movies { get; set; }
    
}