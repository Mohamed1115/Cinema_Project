namespace MVC_PRJ_F.Models;

public class MoveCategory
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public List<Category> Categories { get; set; }
    public int MovieId { get; set; }
    public List<Movie> Movies { get; set; }
}