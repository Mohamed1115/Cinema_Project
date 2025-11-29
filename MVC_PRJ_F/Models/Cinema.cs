namespace MVC_PRJ_F.Models;

public class Cinema
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Image { get; set; }
    public List<Hall> Halls { get; set; }
    public List<CinemaMovies> Movies { get; set; }
    // public List<CinemaMovies> Cinemas { get; set; }
    
}