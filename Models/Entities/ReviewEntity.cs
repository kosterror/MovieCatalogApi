using System.ComponentModel.DataAnnotations;

namespace MovieCatalogApi.Models.Entities;

public class ReviewEntity
{
    public Guid Id { get; set; }
    [Required]
    public virtual UserEntity User { get; set; }
    [Required]
    public virtual MovieEntity Movie { get; set; }
    public string ReviewText { get; set; }          
    [Range(0, 10)]
    public int Rating { get; set; }
    [Required]
    public bool IsAnonymous { get; set; }
    [Required]
    public DateTime CreatedDateTime { get; set; }
}