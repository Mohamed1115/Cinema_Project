namespace MVC_PRJ_F.Models;

public class MoveActor
{
    public int Id { get; set; }
    public int ActorId { get; set; }
    public Actor Actor { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}