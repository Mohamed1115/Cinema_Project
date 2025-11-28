using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_PRJ_F.ModelView;

public class RegisterVM
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Country is required")]
    public string Country { get; set; }  // غيرتها من List إلى string
    
    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }  // غيرتها من List إلى string
    
    // للـ dropdown options في الـ View
    public List<string> CountryList { get; set; } = new();
    public List<string> CityList { get; set; } = new();
    
    

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Please confirm your password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "You must accept the terms")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms")]
    public bool AcceptTerms { get; set; }
}