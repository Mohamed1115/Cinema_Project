namespace MVC_PRJ_F.Models;

public class MoveCategory
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}