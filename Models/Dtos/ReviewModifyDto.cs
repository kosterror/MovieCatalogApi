using System.ComponentModel.DataAnnotations;

namespace MovieCatalogApi.Models.Dtos;

public class ReviewModifyDto
{
    [MaxLength(1000)]           
    public string reviewText { get; set; }
 
    [Required]
    [Range(0, 10)]
    public int rating { get; set; }
    
    [Required]
    public bool isAnonymous { get; set; }
}