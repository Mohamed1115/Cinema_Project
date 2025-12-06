using System.ComponentModel.DataAnnotations;

namespace MVC_PRJ_F.Models;

public class Movie
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Movie title is required")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Movie description is required")]
    public string Description { get; set; }
    
    public bool Status { get; set; }
    
    // MainImage is optional - no [Required] attribute
    public string? MainImage { get; set; }
    
    [Required(ErrorMessage = "Movie duration is required")]
    [Range(1, 10, ErrorMessage = "Duration must be between 1 and 10 hours")]
    public int Time  { get; set; }
    
    public List<MovImage>  SubImages { get; set; }
    public List<MoveActor>? Actors { get; set; }
    public List<MoveCategory>? Categories { get; set; }
}