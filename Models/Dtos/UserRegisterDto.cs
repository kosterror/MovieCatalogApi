using System.ComponentModel.DataAnnotations;

namespace MovieCatalogApi.Models.Dtos;

public class UserRegisterDto
{
    [Required]
    [MinLength(5)]
    public string userName { get; set; }
    
    [Required]
    public string name { get; set; }
    
    [Required]
    [MinLength(6)]
    public string password { get; set; } 
    
    [Required]
    [MinLength(6)]
    [RegularExpression(@"[a-zA-Z]+\w*@[a-zA-Z]+\.[a-zA-Z]+")]
    public string email { get; set; }
    
    [Required]
    public DateTime birthDate { get; set; }
    
    [Required]
    public Gender gender { get; set; }
}