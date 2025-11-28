namespace MVC_PRJ_F.Models;

public class Actor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Image { get; set; }
    public List<MoveActor> Movies { get; set; }
    
}