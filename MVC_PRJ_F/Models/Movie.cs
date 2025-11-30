namespace MVC_PRJ_F.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    // public int Price { get; set; }
    public bool Status { get; set; }
    public string MainImage { get; set; }
    public DateTime DateTime  { get; set; }
    // public int CategoryId { get; set; }
    // public Category Category { get; set; }
    public List<MovImage>  SubImages { get; set; }
    public List<MoveActor> Actors { get; set; }
    public List<MoveCategory> Categories { get; set; }
    
    
}